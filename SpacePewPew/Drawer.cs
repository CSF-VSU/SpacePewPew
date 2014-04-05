using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using SpacePewPew.UI;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.DevIl;

namespace SpacePewPew
{
    public class Drawer
    {

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

            Glu.gluOrtho2D(0.0, Consts.RIGHT, 100.0, 0.0);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            #endregion
            #region TexInitialize
            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);
            Textures = new Dictionary<string, uint>();
            TexInit("BackgroundTexture.jpg", "Main Menu");
            TexInit("BattleMap.jpg", "Battle Map");
           #endregion TexInitialize;
        }

        public Dictionary<string, uint> Textures{ get; set; }

        public PointF[,] CellCoors { get; set; }

        public PointF lightenedCell { get; set; }
  
        
        public void Draw(GameState gameState, LayoutManager manager)
        {
            switch(manager.ScreenType)
            {
                #region MainMenuCase
                case ScreenType.MainMenu:
                    {
                        
                        DrawTexture(Textures["Main Menu"], new PointF(0, 0), Additional.NewPoint(new PointF(Consts.OGL_WIDTH, Consts.OGL_HEIGHT)));
                        foreach (var button in manager.Buttons.Values)
                        {
                            DrawButton(button.Position);
                        }

                        foreach (var btn in manager.Buttons)
                        {
                            drawString(new PointF(btn.Value.Position.X + 2, btn.Value.Position.Y + 4), btn.Key);
                        }
                        break;
                        
                    }
                    #endregion
                #region GameMenuCase
                case ScreenType.GameMenu:
                    {
                            DrawTexture(Textures["Battle Map"], new PointF(0, 0), Additional.NewPoint(new PointF(Consts.OGL_WIDTH, Consts.OGL_HEIGHT)));
                            DrawField(Consts.MAP_START_POS);//lightenedCell);
                            break;
                    }
                #endregion
            }
           
        }

        #region textureDrawing
        private void TexInit(string TexName, string TexDictName)
        {



            Textures[TexDictName] = (uint)Textures.Count - 1;
            //   Il.ilGenImages(1, out imageId);
            //   Il.ilBindImage(imageId);
            if (Il.ilLoadImage(TexName))
            {
                int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
                switch (bitspp)
                {
                    case 24:
                        Textures[TexDictName] = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                        break;
                    case 32:
                        Textures[TexDictName] = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
                        break;
                }

            }
        }

        private uint MakeGlTexture(int Format, IntPtr pixels, int w, int h) 
        {
            // индетефекатор текстурного объекта
            uint texObject;

            // генерируем текстурный объект
            Gl.glGenTextures(1, out texObject);

            // устанавливаем режим упаковки пикселей
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);

            // создаем привязку к только что созданной текстуре
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);

            // устанавливаем режим фильтрации и повторения текстуры
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);

            // создаем RGB или RGBA текстуру
            switch (Format)
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

        private void DrawTexture(uint texture, PointF a, PointF b)
        {

                
                // очистка буфера цвета и буфера глубины 
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                Gl.glClearColor(255, 255, 255, 1);
                // очищение текущей матрицы 
                Gl.glLoadIdentity();
                // включаем режим текстурирования
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                // включаем режим текстурирования , указывая индификатор mGlTextureObject
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);// Textures["Main Menu"]);
                // сохраняем состояние матрицы
                Gl.glPushMatrix();
                Gl.glBegin(Gl.GL_QUADS);
                // указываем поочередно вершины и текстурные координаты
                Gl.glVertex2d(a.X, a.Y);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex2d(b.X, a.Y);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex2d(b.X, b.Y);
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex2d(a.X, b.Y);
                Gl.glTexCoord2f(0, 1);

                // завершаем отрисовку
                Gl.glEnd();

                // возвращаем матрицу
                Gl.glPopMatrix();
                // отключаем режим текстурирования
                Gl.glDisable(Gl.GL_TEXTURE_2D);

                // обновлеям элемент со сценой
                //OGL.Invalidate();
            
        }
        #endregion

        #region buttonDrawing
        private void DrawButton(PointF pos)
        {
            Gl.glColor3f(0.5f, 0.5f, 0.5f);

            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex2d(pos.X, pos.Y);
            Gl.glVertex2d(pos.X + Consts.BUTTON_WIDTH, pos.Y);
            Gl.glVertex2d(pos.X + Consts.BUTTON_WIDTH, pos.Y + Consts.BUTTON_HEIGHT);
            Gl.glVertex2d(pos.X, pos.Y + Consts.BUTTON_HEIGHT);
            Gl.glEnd();
            Gl.glFlush();

            Gl.glColor3f(1, 1, 0.3f);

            Gl.glLineWidth(2);
            Gl.glBegin(Gl.GL_LINE_STRIP);
           
            Gl.glVertex2d(pos.X, pos.Y);
            Gl.glVertex2d(pos.X + Consts.BUTTON_WIDTH, pos.Y);
            Gl.glVertex2d(pos.X + Consts.BUTTON_WIDTH, pos.Y + Consts.BUTTON_HEIGHT);
            Gl.glVertex2d(pos.X, pos.Y + Consts.BUTTON_HEIGHT);
            Gl.glVertex2d(pos.X, pos.Y);
            Gl.glEnd();
            Gl.glLineWidth(1);
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

        private void DrawField(PointF pos)
        {
            for (int i = 0; i < Consts.MAP_WIDTH; i++)
                for (int j = 0; j < Consts.MAP_HEIGHT; j++)
                    if (i % 2 == 0)
                    {
                        CellCoors[i, j] = new PointF(Consts.MAP_START_POS.X + i / 2 * 3 * Consts.CELL_SIDE, Consts.MAP_START_POS.Y + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE);
                        DrawCell(CellCoors[i, j]);
                    }
                    else
                    {
                        CellCoors[i, j] = new PointF(Consts.MAP_START_POS.X + (1.5f + (i - 1) / 2 * 3) * Consts.CELL_SIDE, Consts.MAP_START_POS.Y + (float)(Math.Sqrt(3) * Consts.CELL_SIDE / 2) + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE);
                        DrawCell(CellCoors[i, j]);
                    }
            Gl.glLineWidth(3);
            DrawCell(lightenedCell);//new PointF(lightened.X + Consts.MAP_START_POS.X, lightened.Y + Consts.MAP_START_POS.Y));
            Gl.glLineWidth(1);
         
        }
        #endregion

        #region fontDrawing
        public void drawString(PointF pos, string text)
        {
            Gl.glRasterPos2d(pos.X, pos.Y);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_9_BY_15, text);
        }
        #endregion 
    }
}
