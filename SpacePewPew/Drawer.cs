using System.Drawing;
using SpacePewPew.GameObjects.GameMap;
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
        #region Declarations

        public Dictionary<string, uint> Textures { get; set; }
        public PointF[,] CellCoors { get; set; }
        #endregion

        #region Singleton pattern

        private static Drawer _instance;

        protected Drawer()
        {
            CellCoors = new PointF[Consts.MAP_WIDTH, Consts.MAP_HEIGHT];
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
            TexInit(@"..\..\ShipModels\Korab.png", "Ship");                     
            TexInit(@"..\..\ShipModels\redKorab.png", "ShipColor");             
            TexInit(@"..\..\ShipModels\greenKorab.png", "ShipColorGreen");      
            #endregion TexInitialize;
        }

        public void Draw(GameState gameState, LayoutManager manager, IMap map, int deg)
        {
            PointF[] coordinates;
            switch (manager.ScreenType)
            {
                #region MainMenuCase

                case ScreenType.MainMenu:
                    {
                        coordinates = new[] { new PointF(0, 0), Additional.NewPoint(new PointF(Consts.OGL_WIDTH, 0)), Additional.NewPoint(new PointF(Consts.OGL_WIDTH, Consts.OGL_HEIGHT)), Additional.NewPoint(new PointF(0, Consts.OGL_HEIGHT)) };
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

                #region GameMenuCase

                case ScreenType.GameMenu:
                    {
                        coordinates = new[] { new PointF(0, 0), Additional.NewPoint(new PointF(Consts.OGL_WIDTH, 0)), Additional.NewPoint(new PointF(Consts.OGL_WIDTH, Consts.OGL_HEIGHT)), Additional.NewPoint(new PointF(0, Consts.OGL_HEIGHT)) };
                        DrawTexture(Textures["Battle Map"], coordinates);
                        DrawField(map.LightenedCell);

                        Gl.glEnable(Gl.GL_BLEND);
                        // ur ship goes hia
                        int k = 0;
                        k += deg;

                        var coor = rotate(k, 10, 75, 45);
                        DrawTexture(Textures["Ship"], coor);

                        DrawTexture(Textures["ShipColor"], coor);

                        coor = rotate(20, 10, 45, 25);
                        DrawTexture(Textures["Ship"], coor);

                        DrawTexture(Textures["ShipColorGreen"], coor);
                        Gl.glDisable(Gl.GL_BLEND);

                        DrawStatusBar();
                        break;
                    }
                #endregion
            }
        }

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

    }
}
