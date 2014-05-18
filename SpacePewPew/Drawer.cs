using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using SpacePewPew.GameLogic;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.GameObjects.MapObjects;
using SpacePewPew.Players.Strategies;
using SpacePewPew.UI;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
﻿using System;
﻿using System.Collections.Generic;
using System.Threading;

namespace SpacePewPew
{
    public class Drawer
    {
        public class Bullet
        {
            public PointF Pos { get; set; }
            public bool IsMine { get; set; }
            public int Timeleft { get; set; }
            public int Damage { get; set; }
            public int ShowTime { get; set; }

            public Bullet(bool isMine, PointF pos, int timeleft, int damage)
            {
                IsMine = isMine;
                Pos = pos;
                Timeleft = timeleft;
                Damage = damage;
                ShowTime = 0;
            }

        }

        public class ShipAttributes
        {
            public ShipAttributes(string texName, PlayerColor color, Point pos, int direction)
            {
                TexName = texName;
                Color = color;
                Direction = direction;
                Pos = pos;
              //  TexSize = texSize;

                switch (TexName)
                {
                    case "Fighter":
                    {
                        TexSize = 8;
                        break;
                    }
                    case "Barge":
                    {
                        TexSize = 15;
                        break;
                    }
                    default:
                    {
                        TexSize = 10;
                        break;
                    }
                }
            }

            public Point Pos { get; set; }
            public string TexName { get; set; }
            public PlayerColor Color { get; set; }
            public int Direction { get; set; }
            public int TexSize { get; private set; }
        }

        #region Declarations

        public Dictionary<string, uint> Textures { get; set; }

        public Dictionary<int, ShipAttributes> ShipsInfo { get; set; }

        private ActionState state;
        public PointF LightenedCell { get; set; }
        public PointF[,] CellCoors { get; set; }

        private bool _doneDrawing = true;

        private Point nextCell;
        private int newDir;
        private int _shipId = -1;


        private Point Destination;
        private Decision processingDecision;
        #endregion

        #region Singleton pattern

        private static Drawer _instance;

        protected Drawer()
        {
            CellCoors = new PointF[Consts.MAP_WIDTH, Consts.MAP_HEIGHT];
            state = ActionState.None;
            LightenedCell = Consts.MAP_START_POS;
            ShipsInfo = new Dictionary<int, ShipAttributes>();
        }

        public static Drawer Instance()
        {
            return _instance ?? (_instance = new Drawer());
        }

        #endregion

        public void Initialize()
        {
            #region GlutInitialize

            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            Gl.glClearColor(0.8f, 0.8f, 0.8f, 1);

            Gl.glViewport(0, 0, Consts.OGL_WIDTH, Consts.OGL_HEIGHT);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            Glu.gluOrtho2D(0.0, Consts.SCREEN_WIDTH, 100.0, 0.0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            #endregion

            #region TexInitialize

            Textures = new Dictionary<string, uint>();

            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            TexInit(@"..\..\Textures\BackgroundTexture.jpg", "Main Menu");
            TexInit(@"..\..\Textures\BattleMap.jpg", "Battle Map");

            TexInit(@"..\..\ShipModels\Barge.png", "Barge");
            TexInit(@"..\..\ShipModels\BargeRed.png", "BargeRed");
            TexInit(@"..\..\ShipModels\BargeBlue.png", "BargeBlue");

            TexInit(@"..\..\ShipModels\Fighter.png", "Fighter");
            TexInit(@"..\..\ShipModels\FighterRed.png", "FighterRed");
            TexInit(@"..\..\ShipModels\FighterBlue.png", "FighterBlue");



            #endregion TexInitialize;
        }

        public void Draw(LayoutManager manager, IMapView map) //, ref Action act)
        {
            PointF[] coordinates;

            switch (manager.ScreenType)
            {
                #region MainMenuCase

                case ScreenType.MainMenu:
                {
                    coordinates = new[]                     
                    {      
                      new PointF(0, 0), Additional.NewPoint(new PointF(Consts.OGL_WIDTH, 0)),
                        Additional.NewPoint(new PointF(Consts.OGL_WIDTH, Consts.OGL_HEIGHT)),
                        Additional.NewPoint(new PointF(0, Consts.OGL_HEIGHT))
                    };

                    DrawTexture(Textures["Main Menu"], coordinates);
                    
                    foreach (var btn in manager.Components)
                    {   
                        DrawButton((btn.Value as GameButton), btn.Key);
                       // DrawString(new PointF(btn.Value.Position.X + 2, btn.Value.Position.Y + 4), btn.Key);

                    }
                    break;
                }
                #endregion

               #region GameCase

                case ScreenType.Game:
                {
                    coordinates = new[]
                    {
                        new PointF(0, 0), Additional.NewPoint(new PointF(Consts.OGL_WIDTH, 0)),
                        Additional.NewPoint(new PointF(Consts.OGL_WIDTH, Consts.OGL_HEIGHT)),
                        Additional.NewPoint(new PointF(0, Consts.OGL_HEIGHT))
                    };

                    DrawTexture(Textures["Battle Map"], coordinates);

                    DrawField(LightenedCell, map);
            
                    Gl.glEnable(Gl.GL_BLEND);

                    for (var i = 0; i < map.MapCells.GetLength(0); i++ )
                        for (var j = 0; j < map.MapCells.GetLength(1); j++)
                            if (map.MapCells[i, j].IsLightened)
                            {
                                Gl.glColor3f(1, 0, 0);                              
                                var t = CellToScreen(new Point(i, j));    
                           
                                DrawCell(t);
                            }
                    DrawAction(map);

                    foreach (var a in ShipsInfo)
                        DrawShip(a.Value, map);
                    

                    Gl.glDisable(Gl.GL_BLEND);

                    DrawStatusBar(manager);

                    if ((manager.Components["Shop Menu"] as ListView).Visible)
                    {
                        DrawListView(manager);
                    }
             
                    Animate(map);
                }

                break;
               #endregion
            }
        }


        #region coordinatesConvertation
        public Point ScreenToCell(PointF p)
        {
            for (var i = 0; i < Consts.MAP_WIDTH; i++)
                for (var j = 0; j < Consts.MAP_HEIGHT; j++)
                {
                    PointF center;
                    if (i % 2 == 0)
                        center = new PointF(Consts.MAP_START_POS.X + i / 2 * 3 * Consts.CELL_SIDE + Consts.CELL_SIDE, Consts.MAP_START_POS.Y + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE + Consts.CELL_SIDE * (float)Math.Sqrt(3) / 2);
                    else
                        center = new PointF(Consts.MAP_START_POS.X + (1.5f + (i - 1) / 2 * 3) * Consts.CELL_SIDE + Consts.CELL_SIDE, Consts.MAP_START_POS.Y + (float)Math.Sqrt(3) * Consts.CELL_SIDE / 2 + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE + Consts.CELL_SIDE * (float)Math.Sqrt(3) / 2);
                    if (Math.Sqrt(Math.Pow(p.X - center.X, 2) + Math.Pow(p.Y - center.Y, 2)) < Consts.CELL_SIDE)
                    {
                        LightenedCell = new PointF(center.X - Consts.CELL_SIDE, center.Y - Consts.CELL_SIDE * (float)Math.Sqrt(3) / 2);
                        return new Point(i, j);
                    }
                }

            return new Point();
        }

        public PointF CellToScreen(Point p)
        {
            var x = p.X * 1.5f * Consts.CELL_SIDE;
            float y;
            if (p.X % 2 == 0)
                y = (float)Math.Sqrt(3) * p.Y * Consts.CELL_SIDE;
            else
                y = (float)Math.Sqrt(3) * (p.Y + (float)1/2) * Consts.CELL_SIDE;
            return new PointF(x + Consts.MAP_START_POS.X , y + Consts.MAP_START_POS.Y );
        }

        #endregion

        #region textureDrawing

        private void TexInit(string texName, string texDictName)
        {
            if (Il.ilLoadImage(texName))
            {
                var width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                var height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                var bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
                switch (bitspp)
                {
                    case 24:
                        Textures[texDictName] = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                        break;
                    case 32:
                        Textures[texDictName] = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
                        break;
                }
            }
            /*else
            {
                var s =  Il.ilGetError();
                int a = 4;
            }*/
        }

        private uint MakeGlTexture(int format, IntPtr pixels, int w, int h)
        {
            // индетефекатор текстурного объекта
            uint texObject;

            // генерируем текстурный объект
            Gl.glGenTextures(1, out texObject);

            // устанавливаем режим упаковки пикселей
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);

            // создаем привязку к только что созданной текстуре
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);
            //Gl.glTexFilterFuncSGIS
            // устанавливаем режим фильтрации и повторения текстуры
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);

            // создаем RGB или RGBA текстуру
            switch (format)
            {
                case Gl.GL_RGB:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, w, h, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

                case Gl.GL_RGBA:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, w, h, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;
            }

            return texObject;
        }

        private void DrawTexture(uint texture, PointF[] vertices)
        {
            Gl.glClearColor(255, 255, 255, 1);
            //   Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture); // Textures["Main Menu"]);

            Gl.glBegin(Gl.GL_QUADS);
            // указываем поочередно вершины и текстурные координаты
            Gl.glVertex2d(vertices[0].X, vertices[0].Y);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex2d(vertices[1].X, vertices[1].Y);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex2d(vertices[2].X, vertices[2].Y);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex2d(vertices[3].X, vertices[3].Y);
            Gl.glTexCoord2f(0, 1);

            Gl.glEnd();

            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }

        #endregion

        #region UI Drawing

        private void DrawButton(GameButton btn, string Name)
        {
            Gl.glColor3f(0,0,0);
            Rect(btn.Position.X, btn.Position.Y, btn.Position.X + Consts.BUTTON_WIDTH, btn.Position.Y + Consts.BUTTON_HEIGHT);

            Gl.glColor3f(1, 1, 0.3f);
            Frame(btn.Position.X, btn.Position.Y, btn.Position.X + Consts.BUTTON_WIDTH, btn.Position.Y + Consts.BUTTON_HEIGHT);

            DrawString(new PointF(btn.Position.X + 2, btn.Position.Y + 4), Name);

            if (!btn.Enabled)
            {
                Gl.glColor4f(0.5f, 0.5f, 0.5f, 0.7f);

                Gl.glEnable(Gl.GL_BLEND);
                Rect(btn.Position.X, btn.Position.Y, btn.Position.X + Consts.BUTTON_WIDTH, btn.Position.Y + Consts.BUTTON_HEIGHT);  //STOYANOV PROSTO, SRSLY, SO EZ
                Gl.glDisable(Gl.GL_BLEND);
            }
        }

        private void DrawStatusBar(LayoutManager lm)
        {
            //StatusBar background
            Gl.glColor3f(0.5f, 0.5f, 0.5f);

            Rect(0, 0, Consts.SCREEN_WIDTH, Consts.STATUS_BAR_HEIGHT);

            //PlayerName
            Gl.glColor3f(0, 0, 0);
            Rect(5, 1, 35, 6);
            Gl.glColor3f(1, 1, 0.3f);
            DrawString(new PointF(6, 5), PlayerInfo.Color.ToString());
            
            //stations
            Gl.glColor3f(0, 0, 0);
            Rect(45, 1, 60, 6);
            Gl.glColor3f(1, 1, 0.3f);
            DrawString(new PointF(46, 5), PlayerInfo.Ships.ToString());

            //ResourceGain
            Gl.glColor3f(0, 0, 0);
            Rect(70, 1, 85, 6);
            Gl.glColor3f(1, 1, 0.3f);
            DrawString(new PointF(71,5), "+" + PlayerInfo.Ships);

            //ResourceCount
            Gl.glColor3f(0, 0, 0);
            Rect(95, 1, 110, 6);
            Gl.glColor3f(1, 1, 0.3f);
            DrawString(new PointF(96,5), PlayerInfo.Money.ToString());

            //TimeLeft
            Gl.glColor3f(0, 0, 0);
            Rect(120, 1, 135, 6);
            Gl.glColor3f(1, 1, 0.3f);
            DrawString(new PointF(121, 5), Additional.ConvertTime(PlayerInfo.TimeLeft));

            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2);

           // Gl.glColor3f(1, 1, 0.3f);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2d(0, 7);
            Gl.glVertex2d(Consts.SCREEN_WIDTH, 7);
            Gl.glEnd();
            Gl.glLineWidth(1);

            Gl.glColor3f(1, 1, 0.3f);
            Frame(5, 1, 35, 6);
            Frame(45, 1, 60, 6);
            Frame(70, 1, 85, 6);
            Frame(95, 1, 110, 6);

            var saveBtn = lm.Components["Save"] as GameButton;
            DrawButton(saveBtn, "Save");
            //DrawString(new PointF(saveBtn.Position.X + 2, saveBtn.Position.Y + 4), "Save");

            var endTurnBtn = lm.Components["End Turn"] as GameButton;
            DrawButton(endTurnBtn, "End Turn");
        //    DrawString(new PointF(endTurnBtn.Position.X + 2, endTurnBtn.Position.Y + 4), "End Turn");
        }

        private void DrawListView(LayoutManager manager)
        {
            //ListView background
            Gl.glColor4f(0, 0, 0, 0.7f);

            Gl.glEnable(Gl.GL_BLEND);
            Rect(0, Consts.STATUS_BAR_HEIGHT, Consts.SCREEN_WIDTH, Consts.SCREEN_HEIGHT);  //STOYANOV PROSTO, SRSLY, SO EZ
            Gl.glDisable(Gl.GL_BLEND);

            Gl.glColor3f(0.5f, 0.5f, 0.5f);
            PointF pos = (manager.Components["Shop Menu"] as ListView).Position;
            Rect(pos.X, pos.Y, pos.X * 2.5f, Consts.SCREEN_HEIGHT - Consts.STATUS_BAR_HEIGHT / 2);
            Frame(pos.X, pos.Y, pos.X * 2.5f, Consts.SCREEN_HEIGHT - Consts.STATUS_BAR_HEIGHT / 2);
           
            //ListView Buttons
            DrawButton(manager.Components["Quit Shop"] as GameButton, "Quit Shop");
          //  DrawString(new PointF(manager.Components["Quit Shop"].Position.X + 1, manager.Components["Quit Shop"].Position.Y + 4), "Quit Shop");
            DrawButton(manager.Components["Buy Ship"] as GameButton, "Buy Ship");
           // DrawString(new PointF(manager.Components["Buy Ship"].Position.X + 1, manager.Components["Buy Ship"].Position.Y + 4), "Buy Ship");

            var menu = manager.Components["Shop Menu"] as ListView;
            DrawListViewItem(menu.Items[0], menu.Index == 0);
            DrawListViewItem(menu.Items[1], menu.Index == 1);
        }

        public void DrawListViewItem(ListViewItem lvi, bool isSelected)
        {
            Gl.glColor3f(0, 0, 0);
            Rect(lvi.Position.X, lvi.Position.Y, lvi.Position.X + Consts.LISTVIEWITEM_WIDTH, lvi.Position.Y + Consts.LISTVIEWITEM_HEIGHT);
            if (isSelected)
                Frame(lvi.Position.X, lvi.Position.Y, lvi.Position.X + Consts.LISTVIEWITEM_WIDTH, lvi.Position.Y + Consts.LISTVIEWITEM_HEIGHT);
            DrawString(new PointF(lvi.Position.X + 2, lvi.Position.Y + Consts.LISTVIEWITEM_HEIGHT / 2 - 0.5f), lvi.ItemName);
        }

        #endregion

        #region fieldDrawing

        private void DrawCell(PointF pos)
        {
            
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2, pos.Y);
            Gl.glVertex2d(pos.X + 3 * Consts.CELL_SIDE / 2, pos.Y);
            Gl.glVertex2d(pos.X + 2 * Consts.CELL_SIDE, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE / 2);
            Gl.glVertex2d(pos.X + 3 * Consts.CELL_SIDE / 2, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE);
            Gl.glVertex2d(pos.X, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE / 2);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2, pos.Y);
            Gl.glEnd();
        }

        private void DrawField(PointF lightenedCell, IMapView map)
        {
            
            for (var i = 0; i < Consts.MAP_WIDTH; i++)
                for (var j = 0; j < Consts.MAP_HEIGHT; j++)
                {
                    if (i%2 == 0)
                    {
                        CellCoors[i, j] = new PointF(Consts.MAP_START_POS.X + i/2*3*Consts.CELL_SIDE,
                            Consts.MAP_START_POS.Y + j*(float) Math.Sqrt(3)*Consts.CELL_SIDE);
                        Gl.glColor3f(1, 1, 0.3f);
                        DrawCell(CellCoors[i, j]);
                    }
                    else
                    {
                        CellCoors[i, j] = new PointF(Consts.MAP_START_POS.X + (1.5f + (i - 1)/2*3)*Consts.CELL_SIDE,
                            Consts.MAP_START_POS.Y + (float) (Math.Sqrt(3)*Consts.CELL_SIDE/2) +
                            j*(float) Math.Sqrt(3)*Consts.CELL_SIDE);
                        Gl.glColor3f(1, 1, 0.3f);
                        DrawCell(CellCoors[i, j]);
                    }
                    if (map.MapCells[i, j].Obstacle is Dock) DrawDock(new Point(i, j));
                }

            Gl.glLineWidth(3);
            DrawCell(lightenedCell);
            Gl.glLineWidth(1);
        }

        #endregion

        #region fontDrawing

        public void DrawString(PointF pos, string text)
        {
            Gl.glColor3f(1, 1, 0.3f);
            Gl.glRasterPos2d(pos.X, pos.Y);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_9_BY_15, text);
        }

        #endregion

        #region additionalDrawing
        private void Rect(float x0, float y0, float x1, float y1)
        {
            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex2d(x0, y0);
            Gl.glVertex2d(x1, y0);
            Gl.glVertex2d(x1, y1);
            Gl.glVertex2d(x0, y1);
            Gl.glEnd();
        }

        private void Frame(float x0, float y0, float x1, float y1)
        {
            Gl.glColor3f(1, 1, 0.3f);
            Gl.glLineWidth(2);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex2d(x0, y0);
            Gl.glVertex2d(x1, y0);
            Gl.glVertex2d(x1, y1);
            Gl.glVertex2d(x0, y1);
            Gl.glVertex2d(x0, y0);
            Gl.glEnd();
            Gl.glLineWidth(1);
        }
        #endregion

        #region shipDrawing

        public void DrawShip(ShipAttributes ship, IMapView map)
        {
            var tmp = CellToScreen(ship.Pos);



            DrawTexture(Textures[ship.TexName], rotate(-ship.Direction, ship.TexSize, tmp.X + Consts.CELL_SIDE, tmp.Y + (float)Math.Sqrt(3) / 2 * Consts.CELL_SIDE));
            //TODO
            DrawTexture(Textures[ship.TexName + ship.Color], rotate(-ship.Direction, ship.TexSize, tmp.X + Consts.CELL_SIDE, tmp.Y + (float)Math.Sqrt(3) / 2 * Consts.CELL_SIDE));
            DrawShipStatus(map);

            //TODO: добавить в атрибуты размер модельки, наладить поворот и прочую хуиту
        }
        
        private void DrawShipStatus(IMapView map)
        {
            var ships = map.GetShipIterator(Game.Instance().CurrentPlayer.Color);
            foreach (var ship in ships)
            {
                
                switch (ship.TurnState)
                {
                    case TurnState.Ready:
                        Gl.glColor3f(0.1f, 0.9f, 0.1f);
                        break;
                    case TurnState.InAction:
                        Gl.glColor3f(0.8f, 0.8f, 0.1f);
                        break;
                    case TurnState.Finished:
                        Gl.glColor3f(0.9f, 0.1f, 0.1f);
                        break;
                }


                var pos = CellToScreen(ShipsInfo[ship.Id].Pos);
                //Gl.glLineWidth(10);
                Gl.glPointSize(5);
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glVertex2f(pos.X, pos.Y);
                Gl.glEnd();
            }
        }


        #endregion
        #region actionDrawing

        private PointF[] rotate(int angle, float side, float translationX, float translationY) //на вход 4 точки, на выход 4 точки
        {
            var a = side / 2;
            PointF[] coors1 =
            {
                new PointF( -a * (float)Math.Cos(angle * Math.PI / 180) + a * (float)Math.Sin(angle * Math.PI / 180), -a * (float)Math.Sin(angle * Math.PI / 180) - a * (float)Math.Cos(angle * Math.PI / 180) ),
                new PointF( a * (float)Math.Cos(angle * Math.PI / 180) + a * (float)Math.Sin(angle * Math.PI / 180), a * (float)Math.Sin(angle * Math.PI / 180) - a * (float)Math.Cos(angle * Math.PI / 180) ),
                new PointF( a * (float)Math.Cos(angle * Math.PI / 180) - a * (float)Math.Sin(angle * Math.PI / 180), a * (float)Math.Sin(angle * Math.PI / 180) + a * (float)Math.Cos(angle * Math.PI / 180) ),
                new PointF(- a * (float)Math.Cos(angle * Math.PI / 180) - a * (float)Math.Sin(angle * Math.PI / 180), - a * (float)Math.Sin(angle * Math.PI / 180) + a * (float)Math.Cos(angle * Math.PI / 180) )
            };
            for (var i = 0; i < coors1.Length; i++)
            {
                coors1[i].X += translationX; coors1[i].Y += translationY;
            }

            return coors1;
        }

        private PointF[] elementaryRotate(char dir, float side, float translationX, float translationY)   //dir: - counterclockwise, + clockwise
        {
            PointF[] coors;
            coors = dir == '+' ? rotate(20, side, translationX, translationY) : rotate(-20, side, translationX, translationY);
            return coors;
        }

        private int getNewDirection(Point a, Point b)  //a - old direction, b - new
        {
            switch (a.X % 2)
            {
                case 0:
                    if (b.Y == a.Y)
                        return b.X > a.X ? 240 : 120;
                    if (b.Y < a.Y)
                        if (b.X > a.X) return 300;
                        else if (b.X < a.X) return 60;
                        else return 0;
                    return 180;

                case 1:
                    if (b.Y == a.Y)
                        return b.X > a.X ? 300 : 60;
                    if (b.Y <= a.Y) return 0;
                    if (b.X > a.X) return 240;
                    return b.X < a.X ? 120 : 180;
            }
            return 0;
        }

        private PointF ElementaryTranslate(PointF a, PointF b, int multyplier)
        {
            return new PointF(a.X + (b.X - a.X) * multyplier / 10, a.Y + (b.Y - a.Y) * multyplier / 10);
        }

        
        private void DrawAction(IMapView map)
        {
            switch (state)
            {
                case ActionState.None:
                    {
                        GetShipsFromIMapView(map);
                        if (map.MapCells[map.ChosenShip.X, map.ChosenShip.Y].Ship != null)
                            _shipId = map.MapCells[map.ChosenShip.X, map.ChosenShip.Y].Ship.Id;
                        else
                            _shipId = -1;
                        break;
                    }

                case ActionState.Rotating:
                    {
                        Rotate(map, _shipId);
                        break;
                    }
                case ActionState.Moving:
                    {
                        Move(map, _shipId);
                        break;
                    }
                case ActionState.Attack:
                    Attack(map, _shipId);
                    break;
            }

            if (state == ActionState.None)
            {
                Game.Instance().IsResponding = true;
            }
        }

        private void GetShipsFromIMapView(IMapView map)
        {
            for (var i = 0; i < Consts.MAP_WIDTH; i++)
                for (var j = 0; j < Consts.MAP_HEIGHT; j++)
                {
                    if (map.MapCells[i, j].Ship != null)
                    {
                        ShipAttributes tmp1;
                        if (ShipsInfo.TryGetValue(map.MapCells[i, j].Ship.Id, out tmp1)) //сукасукасука!
                            ShipsInfo[map.MapCells[i, j].Ship.Id].Pos = new Point(i, j);
                            // = new ShipAttributes("Ship", "ShipColor", new Point(i, j), 0); //попровить этот хуец
                        else
                        {
                            switch (map.MapCells[i,j].Ship.Name)
                            {
                                    //TODO: переделать строки в enum или еще что-нибудь
                                case "Fighter":
                                    ShipsInfo[map.MapCells[i, j].Ship.Id] = new ShipAttributes("Fighter",
                                        map.MapCells[i, j].Ship.Color, new Point(i, j), 0);
                                    break;
                                case "Barge":
                                    ShipsInfo[map.MapCells[i, j].Ship.Id] = new ShipAttributes("Barge",
                                        map.MapCells[i, j].Ship.Color, new Point(i, j), 0);
                                    break;
                            }
                        }
                        //ShipsInfo[map.MapCells[i, j].Ship.Id] = new ShipAttributes("Ship",
                        //        map.MapCells[i, j].Ship.Color, new Point(i, j), 0);
                    }
                }
        }

        private void Rotate(IMapView map, int ShipId)
        {
            if (processingDecision.Path.Count != 0)
                newDir = getNewDirection(ShipsInfo[ShipId].Pos,
                    processingDecision.Path[processingDecision.Path.Count - 1]);
            else
            {
                if (processingDecision.DecisionType == DecisionType.Attack)
                    newDir = getNewDirection(ShipsInfo[ShipId].Pos, processingDecision.PointB);
            }

            if (ShipsInfo[ShipId].Direction != newDir)
            {
                ShipsInfo[ShipId].Direction = newDir;
            }

            switch (processingDecision.DecisionType)
            {
                case DecisionType.Attack:
                    state = processingDecision.Path.Count == 0 ? ActionState.Attack : ActionState.Moving;
                    break;
                case DecisionType.Move:
                    state = ActionState.Moving;
                    break;
            }
        }


        private void Move(IMapView map, int ShipId)
        {
            nextCell = processingDecision.Path[processingDecision.Path.Count - 1];
            processingDecision.Path.RemoveAt(processingDecision.Path.Count - 1);

            switch (processingDecision.DecisionType)
            {
                case DecisionType.Attack:
                    ShipsInfo[ShipId].Pos = nextCell;
                    state = ActionState.Rotating;
                    break;
                case DecisionType.Move:
                    ShipsInfo[ShipId].Pos = nextCell;
                    state = ShipsInfo[ShipId].Pos == Destination ? ActionState.None : ActionState.Rotating;
                    break;
            }

            if (nextCell != Destination)
            {
                ShipsInfo[ShipId].Pos = nextCell;

                //TODO : это используется при плавном повороте, которого у нас все равно нет :3
                //newDir = getNewDirection(ShipsInfo[ShipId].Pos, nextCell);
                /*if (!(Math.Abs(ShipsInfo[ShipId].Direction - newDir) <
                      (360 - Math.Abs(ShipsInfo[ShipId].Direction - newDir))))
                    rotateDir *= -1;*/

                state = ActionState.Rotating;
            }
            else
            {
                switch (processingDecision.DecisionType)
                {
                    case DecisionType.Attack:
                        state = ActionState.Rotating;
                        break;
                    case DecisionType.Move:
                        state = ActionState.None;
                        break;
                }
            }
        }

        private void Attack(IMapView map, int attackerShipId) //используется только Decision и карта
        {
            
            Gl.glColor3f(1, 0, 0);

            animation = "PewPew!";
            animationTick = 10 * processingDecision.Battle.Count + 10;
            animationPos = ShipsInfo[processingDecision.ShipIndex].Pos;
            Point shipPos = ShipsInfo[processingDecision.ShipIndex].Pos;
            animationPos = ShipsInfo[attackerShipId].Pos;
            attackerId = attackerShipId;
        /*    if (animationTick == 0)
            {
                if (map.MapCells[shipPos.X, shipPos.Y].Ship == null) // стирание атакованного корабля
                    ShipsInfo.Remove(processingDecision.ShipIndex);

                shipPos = ShipsInfo[attackerShipId].Pos; // стирание атакующего корабля
                if (map.MapCells[shipPos.X, shipPos.Y].Ship == null)
                    ShipsInfo.Remove(attackerShipId);
            } */
            state = ActionState.None;
        }
        #endregion

        #region Animation


        int animationTick = 0;
        string animation = "none";
        Point animationPos = new Point(0, 0);
        public void Animate(IMapView map)
        {   
            
            switch (animation)
            {
                case "BOOM!":
                    {
                        Explotion();
                     //   List<Bullet> bullets = new List<Bullet>() { new Bullet(), new Bullet(), new Bullet() }; 
                        break;
                    }
                case "PewPew!":
                    {
                        Firing(map);
                        break;
                    }
                default: break;

            }

            if (animationTick != 0)
                animationTick--;
            else
                animation = "none";

        }

        public void Explotion()
        {
            
            Gl.glColor3f(1, 0, 0);
            PointF position = CellToScreen(animationPos);
            Random rand = new Random();
            position.X -= Consts.CELL_SIDE / 2;
            position.Y += Consts.CELL_SIDE / 4;

            for (int i = 0; i < 2; i++)
            {
                position.X += (float)rand.Next(-60, 60) / 10;
                position.Y += (float)rand.Next(-60, 60) / 10;
                DrawString(position, "BOOM!");
            }


        }

        List<Bullet> bullets = new List<Bullet>();

        private int attackerId; //говнокод :(
        private int damageNumber = 0;
        public void Firing(IMapView map)
        {
            PointF attacker = CellToScreen(ShipsInfo[attackerId].Pos);
            PointF target = CellToScreen(ShipsInfo[processingDecision.ShipIndex].Pos);
            
            target.X += 3/2*Consts.CELL_SIDE;
            target.Y += (float) Math.Sqrt(3) / 2 * Consts.CELL_SIDE;
            attacker.X += 3 / 2 * Consts.CELL_SIDE;
            attacker.Y += (float) Math.Sqrt(3) / 2 * Consts.CELL_SIDE;

            var Delta = new PointF((target.X - attacker.X)/20, (target.Y - attacker.Y)/20);

            var battle = processingDecision.Battle;
            //стрельба блядь поочередная сука
            int pause = 0;
            if (bullets.Count == 0)
                
                foreach (var step in battle)
                {
                    if (step.IsMineAttack)
                        bullets.Add(new Bullet(true, attacker, pause, step.Damage));
                    else
                        bullets.Add(new Bullet(false, target, pause, step.Damage));
                    pause += 10;
                }


            if (animationTick == 0)
            {
                bullets.Clear();

                Point shipPos = ShipsInfo[processingDecision.ShipIndex].Pos;

                
                if (map.MapCells[shipPos.X, shipPos.Y].Ship == null) // стирание атакованного корабля
                {
                    animation = "BOOM!";
                    animationTick = 30;
                    animationPos = shipPos;
                    ShipsInfo.Remove(processingDecision.ShipIndex);
                }

                shipPos = ShipsInfo[attackerId].Pos; // стирание атакующего корабля
                if (map.MapCells[shipPos.X, shipPos.Y].Ship == null)
                {
                    animation = "BOOM!";
                    animationTick = 30;
                    animationPos = shipPos;
                    ShipsInfo.Remove(attackerId);
                    
                    //animationPos = shipPos;
                   // ShipsInfo.Remove(processingDecision.ShipIndex);

                }
            }



            foreach (var bullet in bullets)
            {
                //   if (bullet.Timeleft == 0) bullet.Exists = false;
                //    if (bullet.Exists)
                if (bullet.Timeleft == 0)
                {
                    if (bullet.IsMine)
                    {
                        if (!IsAround(bullet.Pos, target, 0.5f))
                            bullet.Pos = new PointF(bullet.Pos.X + Delta.X, bullet.Pos.Y + Delta.Y);
                        else
                        {
                            bullet.Timeleft = 100;
                            //DrawString(bullet.Pos, bullet.Damage.ToString());
                            bullet.ShowTime = 8;
                        }
                    }
                    else if (!IsAround(bullet.Pos, attacker, 0.5f))
                        bullet.Pos = new PointF(bullet.Pos.X - Delta.X, bullet.Pos.Y - Delta.Y);
                    else
                    {
                        bullet.Timeleft = 100;
                        bullet.ShowTime = 8;
                    }

                    DrawBullet(bullet.Pos);  
                }
                else
                {
                    bullet.Timeleft--;

                }

                
                if (bullet.ShowTime != 0)
                {
                    
                    DrawString(bullet.Pos, bullet.Damage.ToString());
                    bullet.ShowTime--;
                }

            }
        }

        private bool IsAround(PointF a, PointF b, float delta)
        {
            return (Math.Abs(a.X - b.X) < delta && Math.Abs(a.Y - b.Y) < delta);
        }

        private void DrawBullet(PointF pos)
        {
           Gl.glPointSize(5);
           Gl.glBegin(Gl.GL_POINTS);
           Gl.glVertex2f(pos.X, pos.Y);
           Gl.glEnd();
        }

        #endregion


        #region objectdrawing

        private void DrawDock(Point p)
        {
            Gl.glColor3f(1, 0, 0);//0.9f, 0.4f, 0.3f);
            Gl.glLineWidth(4);
            PointF pos = CellToScreen(p);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2, pos.Y);
            Gl.glVertex2d(pos.X + 3 * Consts.CELL_SIDE / 2, pos.Y);
            Gl.glVertex2d(pos.X + 2 * Consts.CELL_SIDE, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE / 2);
            Gl.glVertex2d(pos.X + 3 * Consts.CELL_SIDE / 2, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE);
            Gl.glVertex2d(pos.X, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE / 2);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2, pos.Y);
       //     Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2, pos.Y);
            Gl.glEnd();
            Gl.glLineWidth(1);

        }
        #endregion
        //-------------------------------

        public void DecisionHandler(object sender, DecisionArgs e)
        {
            processingDecision = e.Decision;

            state = ActionState.Rotating;

            if (processingDecision.Path.Count != 0)
                Destination = processingDecision.Path[0];
        }
    }
}