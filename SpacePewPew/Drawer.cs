using System.Drawing;
using System.Linq;
using SpacePewPew.GameLogic;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.GameObjects.MapObjects;
using SpacePewPew.Players.Strategies;
using SpacePewPew.UI;
using SpacePewPew.UI.GameListView;
using SpacePewPew.UI.Proxy;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
﻿using System;
﻿using System.Collections.Generic;

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
            public ShipAttributes(string texName, PlayerColor color, Point pos, int direction, HealthBar healthBar)
            {
                TexName = texName;
                Color = color;
                Direction = direction;
                Pos = pos;
                HealthBar = healthBar;

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
            public HealthBar HealthBar { get; set; }
        }

        #region Declarations

        public Dictionary<string, uint> Textures { get; set; }

        public Dictionary<int, ShipAttributes> ShipsInfo { get; set; }

        private ActionState _state;
        public PointF LightenedCell { get; set; }
        public PointF[,] CellCoors { get; set; }
        
        private Point _nextCell;
        private int _newDir;
        private int _shipId = -1;

        private Point _destination;
        private Decision _processingDecision;
        #endregion

        #region Singleton pattern

        private static Drawer _instance;

        protected Drawer()
        {
            CellCoors = new PointF[Consts.MAP_WIDTH, Consts.MAP_HEIGHT];
            _state = ActionState.None;
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


            Textures = new Dictionary<string, uint>();

            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            TexInit(@"..\..\Textures\BackgroundTexture.jpg", "Main Menu");      // 1
            TexInit(@"..\..\Textures\BattleMap.jpg", "Battle Map");             // 2

            TexInit(@"..\..\ShipModels\Barge.png", "Barge");                    // 3
            TexInit(@"..\..\ShipModels\BargeRed.png", "BargeRed");              // 4
            TexInit(@"..\..\ShipModels\BargeBlue.png", "BargeBlue");            // 5

            TexInit(@"..\..\ShipModels\Fighter.png", "Fighter");                // 6
            TexInit(@"..\..\ShipModels\FighterRed.png", "FighterRed");          // 7
            TexInit(@"..\..\ShipModels\FighterBlue.png", "FighterBlue");        // 8
        }

        public void Draw(LayoutManager manager, IMapView map) //, ref Action act)
        {
            var converter = Proxy.GetInstance();
            var coordinates = new[]
                    {
                        new PointF(0, 0), converter.NewPoint(new PointF(Consts.OGL_WIDTH, 0)),
                        converter.NewPoint(new PointF(Consts.OGL_WIDTH, Consts.OGL_HEIGHT)),
                        converter.NewPoint(new PointF(0, Consts.OGL_HEIGHT))
                    };

            switch (manager.ScreenType)
            {
                #region MainMenuCase
                case ScreenType.MainMenu:
                {
                    DrawTexture(Textures["Main Menu"], coordinates);

                    foreach (var item in manager.Components)
                    {
                        item.Draw();
                    }
                    break;
                }
                #endregion

                #region GameCase
                case ScreenType.Game:
                {
                    DrawTexture(Textures["Battle Map"], coordinates);

                    DrawField(map);
            
                    Gl.glEnable(Gl.GL_BLEND);
                    DrawAction(map);

                    foreach (var a in ShipsInfo)
                        DrawShip(a.Value, map);
                    Gl.glDisable(Gl.GL_BLEND);

                    Animate(map);

                    foreach (var item in manager.Components)
                    {
                        item.Draw();
                    }
                    if (!manager.IsShowingModal) return;
                    
                    ObscureScreen();
                    foreach (var item in manager.ModalComponents)
                    {
                        if (item is ListView)
                            (item as ListView).SetItemsEnabledBy(data =>
                                Game.Instance().Races[Game.Instance().CurrentPlayer.Race].BuildShip((int) data.GlyphNum)
                                    .Cost <= Game.Instance().CurrentPlayer.Money
                            );

                        item.Draw();
                    }
                }
                break;
                #endregion
            }
        }

        private void ObscureScreen()
        {
            Gl.glColor4f(0, 0, 0, 0.7f);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex2d(0, 0);
            Gl.glVertex2d(Consts.SCREEN_WIDTH, 0);
            Gl.glVertex2d(Consts.SCREEN_WIDTH, Consts.SCREEN_HEIGHT);
            Gl.glVertex2d(0, Consts.SCREEN_HEIGHT);
            Gl.glEnd();
            Gl.glDisable(Gl.GL_BLEND);
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
                var width  = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
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

        public void DrawTexture(uint texture, PointF[] vertices)
        {
            Gl.glClearColor(255, 255, 255, 1);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);

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

        #region fieldDrawing

        private void DrawCell(PointF pos)
        {
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2.0, pos.Y);
            Gl.glVertex2d(pos.X + 3 * Consts.CELL_SIDE / 2.0, pos.Y);
            Gl.glVertex2d(pos.X + 2 * Consts.CELL_SIDE, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE / 2);
            Gl.glVertex2d(pos.X + 3 * Consts.CELL_SIDE / 2.0, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2.0, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE);
            Gl.glVertex2d(pos.X, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE / 2);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2.0, pos.Y);
            Gl.glEnd();
        }

        private void DrawField(IMapView map)
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
                    if (map.MapCells[i, j].Object is Dock) DrawDock(new Point(i, j));
                }


            Gl.glEnable(Gl.GL_BLEND);

            for (var i = 0; i < map.MapCells.GetLength(0); i++)
                for (var j = 0; j < map.MapCells.GetLength(1); j++)
                    if (map.MapCells[i, j].IsLightened)
                    {
                        Gl.glColor3f(1, 0, 0);
                        var t = CellToScreen(new Point(i, j));

                        DrawCell(t);
                    }
            Gl.glDisable(Gl.GL_BLEND);

            Gl.glColor3f(0, 1, 0);
            Gl.glLineWidth(3);
            DrawCell(LightenedCell);
            Gl.glLineWidth(1);
        }

        #endregion

        #region shipDrawing

        public void DrawShip(ShipAttributes ship, IMapView map)
        {
            var tmp = CellToScreen(ship.Pos);

            DrawTexture(Textures[ship.TexName], rotate(-ship.Direction, ship.TexSize, tmp.X + Consts.CELL_SIDE, tmp.Y + (float)Math.Sqrt(3) / 2 * Consts.CELL_SIDE));
            DrawTexture(Textures[ship.TexName + ship.Color], rotate(-ship.Direction, ship.TexSize, tmp.X + Consts.CELL_SIDE, tmp.Y + (float)Math.Sqrt(3) / 2 * Consts.CELL_SIDE));
            DrawShipStatus(map);
            DrawHealthBar(ship.HealthBar);

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
                Gl.glPointSize(5);
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glVertex2f(pos.X, pos.Y);
                Gl.glEnd();
            }
        }

        private void DrawHealthBar(HealthBar healthBar)
        {
            UiElement.Frame(healthBar.Position.X, healthBar.Position.Y + 2, healthBar.Position.X + 1, healthBar.Position.Y + 10);
            float f = (float) healthBar.CurrentHealth/healthBar.MaxHealth;
            if (f >= 0.5f) 
                Gl.glColor3f(0, 1, 0);
            else if (f > 0.25f && f < 0.5f)
                Gl.glColor3f(1, 1, 0.3f);
            else
                Gl.glColor3f(1, 0, 0);

            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex2f(healthBar.Position.X, healthBar.Position.Y + 10);
            Gl.glVertex2f(healthBar.Position.X + 1, healthBar.Position.Y + 10);
            Gl.glVertex2f(healthBar.Position.X + 1, healthBar.Position.Y + 10 - 8*f);
            Gl.glVertex2f(healthBar.Position.X, healthBar.Position.Y + 10 - 8*f);
            Gl.glEnd();
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

        /*private PointF[] elementaryRotate(char dir, float side, float translationX, float translationY)   //dir: - counterclockwise, + clockwise
        {
            PointF[] coors;
            coors = dir == '+' ? rotate(20, side, translationX, translationY) : rotate(-20, side, translationX, translationY);
            return coors;
        }*/
        /*
        private PointF ElementaryTranslate(PointF a, PointF b, int multyplier)
        {
            return new PointF(a.X + (b.X - a.X) * multyplier / 10, a.Y + (b.Y - a.Y) * multyplier / 10);
        }*/


        private int GetNewDirection(Point a, Point b)  //a - old direction, b - new
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
        
        
        private void DrawAction(IMapView map)
        {
            switch (_state)
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
                        Rotate(_shipId);
                        break;
                    }
                case ActionState.Moving:
                    {
                        Move(_shipId);
                        break;
                    }
                case ActionState.Attack:
                    Attack(_shipId);
                    break;
            }

            if (_state == ActionState.None)
            {
                Game.Instance().IsResponding = true;
            }
        }

        private void GetShipsFromIMapView(IMapView map)
        {
            for (var i = 0; i < Consts.MAP_WIDTH; i++)
                for (var j = 0; j < Consts.MAP_HEIGHT; j++)
                {
                    if (map.MapCells[i, j].Ship == null) continue;
                    
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
                                    map.MapCells[i, j].Ship.Color, new Point(i, j), 0, new HealthBar(map.MapCells[i,j].Ship.MaxHealth, CellToScreen(new Point(i,j))));
                                break;
                            case "Barge":
                                ShipsInfo[map.MapCells[i, j].Ship.Id] = new ShipAttributes("Barge",
                                    map.MapCells[i, j].Ship.Color, new Point(i, j), 0, new HealthBar(map.MapCells[i,j].Ship.MaxHealth, CellToScreen(new Point(i,j))));
                                break;
                        }
                    }
                    //ShipsInfo[map.MapCells[i, j].Ship.Id] = new ShipAttributes("Ship",
                    //        map.MapCells[i, j].Ship.Color, new Point(i, j), 0);
                }
        }

        private void Rotate(int shipId)
        {
            if (_processingDecision.Path.Count != 0)
                _newDir = GetNewDirection(ShipsInfo[shipId].Pos,
                    _processingDecision.Path[_processingDecision.Path.Count - 1]);
            else
            {
                if (_processingDecision.DecisionType == DecisionType.Attack)
                    _newDir = GetNewDirection(ShipsInfo[shipId].Pos, _processingDecision.PointB);
            }

            ShipsInfo[shipId].Direction = _newDir;

            switch (_processingDecision.DecisionType)
            {
                case DecisionType.Attack:
                    _state = _processingDecision.Path.Count == 0 ? ActionState.Attack : ActionState.Moving;
                    break;
                case DecisionType.Move:
                    _state = ActionState.Moving;
                    break;
            }
        }


        private void Move(int ShipId)
        {
            _nextCell = _processingDecision.Path[_processingDecision.Path.Count - 1];
            _processingDecision.Path.RemoveAt(_processingDecision.Path.Count - 1);

            switch (_processingDecision.DecisionType)
            {
                case DecisionType.Attack:
                    ShipsInfo[ShipId].Pos = _nextCell;
                    ShipsInfo[ShipId].HealthBar.Position = CellToScreen(_nextCell);
                    _state = ActionState.Rotating;
                    break;
                case DecisionType.Move:
                    ShipsInfo[ShipId].Pos = _nextCell;
                    ShipsInfo[ShipId].HealthBar.Position = CellToScreen(_nextCell);
                    _state = ShipsInfo[ShipId].Pos == _destination ? ActionState.None : ActionState.Rotating;
                    break;
            }

            if (_nextCell != _destination)
            {
                ShipsInfo[ShipId].Pos = _nextCell;

                //TODO : это используется при плавном повороте, которого у нас все равно нет :3
                //newDir = getNewDirection(ShipsInfo[ShipId].Pos, nextCell);
                /*if (!(Math.Abs(ShipsInfo[ShipId].Direction - newDir) <
                      (360 - Math.Abs(ShipsInfo[ShipId].Direction - newDir))))
                    rotateDir *= -1;*/

                _state = ActionState.Rotating;
            }
            else
            {
                switch (_processingDecision.DecisionType)
                {
                    case DecisionType.Attack:
                        _state = ActionState.Rotating;
                        break;
                    case DecisionType.Move:
                        _state = ActionState.None;
                        break;
                }
            }
        }

        private void Attack(int attackerShipId) //используется только Decision и карта
        {
            Gl.glColor3f(1, 0, 0);

            animation = "PewPew!";
            animationTick = 10 * _processingDecision.Battle.Count + 20;
            animationPos = ShipsInfo[_processingDecision.ShipIndex].Pos;
            animationPos = ShipsInfo[attackerShipId].Pos;
            attackerId = attackerShipId;
      
            _state = ActionState.None;
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
                    Explosion();
                    break;
                }
                case "PewPew!":
                {
                    Firing(map);
                    break;
                }
            }

            if (animationTick != 0)
            {
                animationTick--;
                /*if (!Game.Instance().IsShowingModal)
                    Game.Instance().IsShowingModal = true;*/
            }
            else
            {
                animation = "none";
                //Game.Instance().IsShowingModal = false;
            }
        }

        public void Explosion()
        {
            
            Gl.glColor3f(1, 0, 0);
            var position = CellToScreen(animationPos);
            var rand = new Random();
            position.X -= Consts.CELL_SIDE / 2.0f;
            position.Y += Consts.CELL_SIDE / 4.0f;

            for (var i = 0; i < 2; i++)
            {
                position.X += (float)rand.Next(-60, 60) / 10;
                position.Y += (float)rand.Next(-60, 60) / 10;
                //DrawString(position, "BOOM!");
            }
        }

        readonly List<Bullet> bullets = new List<Bullet>();

        private int attackerId; //TODO: исправить говнокод :(
        private int damageNumber = 0;
        public void Firing(IMapView map)
        {
            var attacker = CellToScreen(ShipsInfo[attackerId].Pos);
            var target = CellToScreen(ShipsInfo[_processingDecision.ShipIndex].Pos);
            
            target.X += 3/2*Consts.CELL_SIDE;
            target.Y += (float) Math.Sqrt(3) / 2 * Consts.CELL_SIDE;
            attacker.X += 3 / 2 * Consts.CELL_SIDE;
            attacker.Y += (float) Math.Sqrt(3) / 2 * Consts.CELL_SIDE;

            var Delta = new PointF((target.X - attacker.X)/20, (target.Y - attacker.Y)/20);

            var battle = _processingDecision.Battle;
            //стрельба блядь поочередная сука
            var pause = 0;
            if (bullets.Count == 0)
                
                foreach (var step in battle)
                {
                    bullets.Add(step.IsMineAttack
                        ? new Bullet(true, attacker, pause, step.Damage)
                        : new Bullet(false, target, pause, step.Damage));
                    pause += 10;
                }


            if (animationTick == 0)
            {
                bullets.Clear();

                var shipPos = ShipsInfo[_processingDecision.ShipIndex].Pos;
                
                if (map.MapCells[shipPos.X, shipPos.Y].Ship == null) // стирание атакованного корабля
                {
                    animation = "BOOM!";
                    animationTick = 30;
                    animationPos = shipPos;
                    ShipsInfo.Remove(_processingDecision.ShipIndex);
                }

                shipPos = ShipsInfo[attackerId].Pos; // стирание атакующего корабля
                if (map.MapCells[shipPos.X, shipPos.Y].Ship == null)
                {
                    animation = "BOOM!";
                    animationTick = 30;
                    animationPos = shipPos;
                    ShipsInfo.Remove(attackerId);
                }
            }

            for (var i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Timeleft == 0)
                {
                    if (bullets[i].IsMine)
                    {
                        if (!IsAround(bullets[i].Pos, target, 0.5f))
                            bullets[i].Pos = new PointF(bullets[i].Pos.X + Delta.X, bullets[i].Pos.Y + Delta.Y);
                        else
                        {
                            bullets[i].Timeleft = 100;
                            bullets[i].ShowTime = 8;
                                var tmp = ShipsInfo.First(ship => ship.Value.Pos == ScreenToCell(target));
                                tmp.Value.HealthBar.CurrentHealth -= battle[i].Damage;
                        }
                    }
                    else if (!IsAround(bullets[i].Pos, attacker, 0.5f))
                        bullets[i].Pos = new PointF(bullets[i].Pos.X - Delta.X, bullets[i].Pos.Y - Delta.Y);
                    else
                    {  
                        bullets[i].Timeleft = 100;
                        bullets[i].ShowTime = 8;
                            var tmp = ShipsInfo.First(ship => ship.Value.Pos == ScreenToCell(attacker));
                            tmp.Value.HealthBar.CurrentHealth -= battle[i].Damage;
                    }

                    DrawBullet(bullets[i].Pos);  
                }
                else
                {
                    bullets[i].Timeleft--;
                }

                if (bullets[i].ShowTime == 0) continue;
                
                //DrawString(bullets[i].Pos, bullets[i].Damage.ToString());
                bullets[i].ShowTime--;
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
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2.0, pos.Y);
            Gl.glVertex2d(pos.X + 3 * Consts.CELL_SIDE / 2.0, pos.Y);
            Gl.glVertex2d(pos.X + 2 * Consts.CELL_SIDE, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE / 2);
            Gl.glVertex2d(pos.X + 3 * Consts.CELL_SIDE / 2.0, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2.0, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE);
            Gl.glVertex2d(pos.X, pos.Y + Math.Sqrt(3) * Consts.CELL_SIDE / 2);
            Gl.glVertex2d(pos.X + Consts.CELL_SIDE / 2.0, pos.Y);
            Gl.glEnd();
            Gl.glLineWidth(1);

        }
        #endregion
        //-------------------------------

        public void DecisionHandler(object sender, DecisionArgs e)
        {
            _processingDecision = e.Decision;

            _state = ActionState.Rotating;

            if (_processingDecision.Path.Count != 0)
                _destination = _processingDecision.Path[0];
        }
    }
}