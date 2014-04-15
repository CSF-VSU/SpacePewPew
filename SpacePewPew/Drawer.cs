using System.Drawing;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.Players.Strategies;
using SpacePewPew.UI;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
﻿using System;
﻿using System.Collections.Generic;

namespace SpacePewPew
{
    public class Drawer
    {
        public class ShipAttributes
        {
            public ShipAttributes(string texName, string color, Point pos, int direction)
            {
                TexName = texName;
                Color = color;
                Direction = direction;
                Pos = pos;
            }

            public Point Pos { get; set; }
            public string TexName { get; set; }
            public string Color { get; set; }
            public int Direction { get; set; }
        }


        #region Declarations

        public Dictionary<string, uint> Textures { get; set; }
        //public Dictionary<Type, string> ShipTex { get; set; }

        public Dictionary<int, ShipAttributes> ShipsInfo { get; set; }

        private ActionState state;
        public PointF LightenedCell { get; set; }
        public PointF[,] CellCoors { get; set; }

        private bool _isResponding;

        // public bool inAction { get; set; } // флаг для движения, поворота и тд


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

            // ShipsInfo[map.MapCells[i, j].Ship.id].Pos = new ShipAttributes("Ship", "ShipColor", new Point(i, j), 0);

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
            TexInit(@"..\..\ShipModels\Korab.png", "Ship");
            TexInit(@"..\..\ShipModels\redKorab.png", "ShipColor");
            TexInit(@"..\..\ShipModels\greenKorab.png", "ShipColorGreen");

            #endregion TexInitialize;
        }

        private Point nextCell;
        private int newDir = 0;
        private int rotateDir = 1;
        private int Count = 0;
        private Point Destination;

        // сохраняй состояние IMap
        // + флажок isDrawing

        public void Draw(GameState gameState, LayoutManager manager, IMap map) //, ref Action act)
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
                    
                    foreach (var button in manager.Buttons.Values)
                    
                    {
                    
                        DrawButton(button.Position);
                    }



                    foreach (var btn in manager.Buttons)
                    {
                        DrawString(new PointF(btn.Value.Position.X + 2, btn.Value.Position.Y + 4), btn.Key);
                    }
                    break;

                }

                    #endregion

                    #region GameCase

                    PointF tmp; //<-- ОСТОРОЖНО, ГОВНОКОД!!!

                case ScreenType.Game:
                {
                    coordinates = new[]
                    {
                        new PointF(0, 0), Additional.NewPoint(new PointF(Consts.OGL_WIDTH, 0)),
                        Additional.NewPoint(new PointF(Consts.OGL_WIDTH, Consts.OGL_HEIGHT)),
                        Additional.NewPoint(new PointF(0, Consts.OGL_HEIGHT))
                    };
                    DrawTexture(Textures["Battle Map"], coordinates);
                    DrawField(LightenedCell);

                    /*if (act.IsReady)
                    {

                        state = ActionState.Rotating;
                        Destination = act.Destination;
                        act.Refresh();

                    }*/


                    Gl.glEnable(Gl.GL_BLEND);

                    switch (state)
                    {
                        case ActionState.None:
                        {
                            for (int i = 0; i < Consts.MAP_WIDTH; i++)
                                for (int j = 0; j < Consts.MAP_HEIGHT; j++)
                                {
                                    if (map.MapCells[i, j].Ship != null)
                                    {
                                        ShipAttributes tmp1;
                                        if (ShipsInfo.TryGetValue(map.MapCells[i, j].Ship.id, out tmp1)) //сукасукасука!
                                            ShipsInfo[map.MapCells[i, j].Ship.id].Pos = new Point(i, j);
                                                // = new ShipAttributes("Ship", "ShipColor", new Point(i, j), 0); //попровить этот хуец
                                        else
                                            ShipsInfo[map.MapCells[i, j].Ship.id] = new ShipAttributes("Ship",
                                                "ShipColor", new Point(i, j), 0);
                                    }
                                }
                            break;
                        }

                        case ActionState.Rotating:
                        {
                            newDir = getNewDirection(ShipsInfo[act.ShipId].Pos, act.Path[act.Path.Count - 1]);
                                //initialization

                            if (ShipsInfo[act.ShipId].Direction != newDir)
                            {
                                tmp = CellToScreen(ShipsInfo[act.ShipId].Pos);
                                /*  if (rotateDir == -1)//(Math.Abs(ShipsInfo[act.ShipId].Direction - newDir) < (360 - Math.Abs(ShipsInfo[act.ShipId].Direction - newDir)))
                                       {
                                           elementaryRotate('-', 10, tmp.X, tmp.Y);
                                           ShipsInfo[act.ShipId].Direction -= 20;
                                       }
                                       else
                                       {
                                          elementaryRotate('+', 10, tmp.X, tmp.Y);
                                          ShipsInfo[act.ShipId].Direction += 20;
                                  
                                       }  */
                                if (rotateDir == -1)
                                    elementaryRotate('-', 10, tmp.X, tmp.Y);
                                else
                                    elementaryRotate('+', 10, tmp.X, tmp.Y);
                                ShipsInfo[act.ShipId].Direction += rotateDir*20;
                                if (ShipsInfo[act.ShipId].Direction >= 360) ShipsInfo[act.ShipId].Direction -= 360;
                                if (ShipsInfo[act.ShipId].Direction < 0) ShipsInfo[act.ShipId].Direction += 360;
                            }
                            else
                            {
                                //act.Refresh();
                                state = ActionState.Moving;
                            }
                            break;
                        }
                        case ActionState.Moving:
                        {
                            Count++;
                            nextCell = act.Path[act.Path.Count - 1];
                            act.Path.RemoveAt(act.Path.Count - 1);
                            newDir = getNewDirection(ShipsInfo[act.ShipId].Pos, nextCell);

                            ShipsInfo[act.ShipId].Pos = nextCell;

                            if (nextCell != Destination)
                            {
                                //  if (ShipsInfo[act.ShipId].Direction < 0) ShipsInfo[act.ShipId].Direction += 360;
                                if (
                                    !(Math.Abs(ShipsInfo[act.ShipId].Direction - newDir) <
                                      (360 - Math.Abs(ShipsInfo[act.ShipId].Direction - newDir))))
                                    rotateDir *= -1;
                                state = ActionState.Rotating;
                            }
                            else
                            {
                                //act.Refresh();
                                state = ActionState.None;
                            }

                            break;
                        }

                    }
                }

                    // PointF tmp;
                    foreach (var a in ShipsInfo)
                    {
                        tmp = CellToScreen(a.Value.Pos);
                        //  tmp = elementaryTranslate(tmp, CellToScreen(nextCell), Count);
                        DrawTexture(Textures[a.Value.TexName], rotate(- a.Value.Direction, 10, tmp.X, tmp.Y));
                        DrawTexture(Textures[a.Value.Color], rotate(- a.Value.Direction, 10, tmp.X, tmp.Y));
                    }

                    Gl.glDisable(Gl.GL_BLEND);

                    DrawStatusBar();
                    break;
            }

            #endregion
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
            float x = p.X * 1.5f * Consts.CELL_SIDE;
            float y;
            if (p.X % 2 == 0)
                y = (float)Math.Sqrt(3) * p.Y * Consts.CELL_SIDE;
            else
                y = (float)Math.Sqrt(3) * (p.Y + (float)1/2) * Consts.CELL_SIDE;
            return new PointF(x + Consts.MAP_START_POS.X + Consts.CELL_SIDE, y + Consts.MAP_START_POS.Y + Consts.CELL_SIDE * (float)Math.Sqrt(3)/2);
        }

        #endregion


        #region textureDrawing

        private void TexInit(string texName, string texDictName)
        {
            if (Il.ilLoadImage(texName))
            {
                int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
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

        private void DrawButton(PointF pos)
        {
            Gl.glColor3f(0.5f, 0.5f, 0.5f);
            Rect(pos.X, pos.Y, pos.X + Consts.BUTTON_WIDTH, pos.Y + Consts.BUTTON_HEIGHT);

            Gl.glColor3f(1, 1, 0.3f);
            Frame(pos.X, pos.Y, pos.X + Consts.BUTTON_WIDTH, pos.Y + Consts.BUTTON_HEIGHT);
        }



        private void DrawStatusBar()
        {
            //StatusBar background
            Gl.glColor3f(0.5f, 0.5f, 0.5f);
            Rect(0, 0, Consts.SCREEN_WIDTH, 7);

            //PlayerName
            Gl.glColor3f(0, 0, 0);
            Rect(5, 1, 35, 6);

            //stations
            Rect(45, 1, 60, 6);

            //ResourceGain
            Rect(70, 1, 85, 6);

            //ResourceCount
            Rect(95, 1, 110, 6);

            Gl.glLineWidth(2);
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

            DrawString(new PointF(7, 4.5f), "Player Name");
            DrawString(new PointF(47, 4.5f), string.Format("{0} st.", 0));
            DrawString(new PointF(71.5f, 4.5f), string.Format("+{0} res", 0));
            DrawString(new PointF(97, 4.5f), string.Format("{0} res", 0));
        }

        #endregion

        #region fieldDrawing

        private void DrawCell(PointF pos)
        {
            Gl.glColor3f(1, 1, 0.3f);
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

        private void DrawField(PointF lightenedCell)
        {
            for (var i = 0; i < Consts.MAP_WIDTH; i++)
                for (var j = 0; j < Consts.MAP_HEIGHT; j++)
                    if (i % 2 == 0)
                    {
                        CellCoors[i, j] = new PointF(Consts.MAP_START_POS.X + i / 2 * 3 * Consts.CELL_SIDE,
                            Consts.MAP_START_POS.Y + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE);
                        DrawCell(CellCoors[i, j]);
                    }
                    else
                    {
                        CellCoors[i, j] = new PointF(Consts.MAP_START_POS.X + (1.5f + (i - 1) / 2 * 3) * Consts.CELL_SIDE,
                            Consts.MAP_START_POS.Y + (float)(Math.Sqrt(3) * Consts.CELL_SIDE / 2) +
                            j * (float)Math.Sqrt(3) * Consts.CELL_SIDE);
                        DrawCell(CellCoors[i, j]);
                    }
            Gl.glLineWidth(3);
            DrawCell(lightenedCell);
            Gl.glLineWidth(1);
        }

        #endregion

        #region fontDrawing

        public void DrawString(PointF pos, string text)
        {
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

        #endregion


        //-------------------------------

        public void DecisionHandler(object sender, DecisionArgs e)
        {
            // VESHAI FLAZHKI O TOM SHTO NOT RESPONDENG
            // do drawing
        }
    }
}
