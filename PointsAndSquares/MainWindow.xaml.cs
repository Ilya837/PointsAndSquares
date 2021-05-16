using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PointsAndSquares
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double X, Y; //координаты левого верхнего угла игровой зоны
        double H, W;//Длина и высота игровой зоны
        double L, T;//Длина и толщина прямоугольника, на основе которого строятся полигоны
        const double k = 4;//Коэффициент между длинной и толщиной прямоугольника 
        double d;//  дельта(длина + толщина)
        int C1, C2,C = 3;//Размер поля (в клетках) длина и ширина
        const double del = 40; // Размер отступа
        int player ; // Номер игрока, который ходит
        int b,r, sc;

        

        Polygon[][][] P;
        Rectangle[][] R;
        int[][] Col;

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;

            C1 = C2 = C;
            X = 0; Y = 0;
            H = W = Math.Min(wind.Height - 2*del, wind.Width - 2 * del);
            grid1.Height = wind.Height;
            grid1.Width = wind.Width;
            player = 0;
            b = 0;
            r = 0;
            sc = 0;

            if (H <= W)
            {
                Y = del; X = (wind.Width - H) / 2;
            }
            else
            {
                X = del; Y = (wind.Height - W) / 2;
            }

            T = W / (k * C1 + C1 + 1);
            L = T * k;
            d = T + L;

            Col = new int[C1][];
            for(int i = 0; i< C1; i++)
            {
                Col[i] = new int[C2];
            }


            P = new Polygon[C1][][];
            for (int i = 0; i < C1; i++)
            {
                P[i] = new Polygon[C2 + 1][];
            }
            for (int i = 0; i < C1; i++)
                for (int j = 0; j < C2 + 1; j++)
                {
                    P[i][j] = new Polygon[2];
                }



            for (int i = 0; i < C1; i++)
            {
                for (int j = 0; j < C2 + 1; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        if (k == 0)
                        {
                            P[i][j][k] = new Polygon();
                            P[i][j][k].Points = new PointCollection();
                            P[i][j][k].Points.Add(new Point(X + T / 2 + d * i, Y + T / 2 + d * j));
                            P[i][j][k].Points.Add(new Point(X + T + d * i, Y + d * j));
                            P[i][j][k].Points.Add(new Point(X + d * (i + 1), Y + d * j));
                            P[i][j][k].Points.Add(new Point(X + T / 2 + d * (i + 1), Y + T / 2 + d * j));
                            P[i][j][k].Points.Add(new Point(X + d * (i + 1), Y + T + d * j));
                            P[i][j][k].Points.Add(new Point(X + T + d * i, Y + T + d * j));
                            P[i][j][k].Stroke = Brushes.Black;
                            P[i][j][k].Fill = Brushes.White;
                            P[i][j][k].MouseLeftButtonDown += Mouse_Click;
                            grid1.Children.Add(P[i][j][k]);
                        }
                        else
                        {
                            P[i][j][k] = new Polygon();
                            P[i][j][k].Points = new PointCollection();
                            P[i][j][k].Points.Add(new Point(X + d * j, Y + T + d * i));
                            P[i][j][k].Points.Add(new Point(X + T / 2 + d * j, Y + T / 2 + d * i));
                            P[i][j][k].Points.Add(new Point(X + T + d * j, Y + T + d * i));
                            P[i][j][k].Points.Add(new Point(X + T + d * j, Y + d * (i + 1)));
                            P[i][j][k].Points.Add(new Point(X + T / 2 + d * j, Y + T / 2 + d * (i + 1)));
                            P[i][j][k].Points.Add(new Point(X + d * j, Y + d * (i + 1)));
                            P[i][j][k].Fill = Brushes.White;
                            P[i][j][k].MouseLeftButtonDown += Mouse_Click;
                            P[i][j][k].Stroke = Brushes.Black;
                            
                            grid1.Children.Add(P[i][j][k]);
                        }
                    }
                }
            }

            R = new Rectangle[C1][];
            for (int i = 0; i < C1; i++)
            {
                R[i] = new Rectangle[C2];
            }

            for (int i = 0; i < C1; i++)
            {
                for (int j = 0; j < C2; j++)
                {
                    R[i][j] = new Rectangle();
                    R[i][j].Height = L;
                    R[i][j].Width = L;
                    R[i][j].Stroke = Brushes.Black;
                    //R[i][j].Fill = Brushes.White;
                    R[i][j].RenderTransform = new TranslateTransform(-grid1.Width/2 + R[i][j].Width/ 2 + X + T + j * d, -grid1.Height/2 + R[i][j].Height / 2 + Y + T + i * d);

                    grid1.Children.Add(R[i][j]);
                }
            }


        }

        private void Mouse_Click(object sender, MouseButtonEventArgs e)
        {
            Polygon p = (Polygon)sender;
            int i, j, k;
            bool w = false;

            if(player == 0)
            {
                p.Fill = Brushes.Blue;
            }
            else
            {
                p.Fill = Brushes.Red;
            }

            if( Math.Abs(Math.Round((p.Points[4].X - X) / d)  - ((p.Points[4].X - X) / d)) < 0.0000001)
            {
                k = 0;
            }
            else
            {
                k = 1;
            }

            if(k == 0)
            {
                j = (int)Math.Round((p.Points[4].X - X)/d - 1);
                i = (int)Math.Round((p.Points[1].Y - Y) / d);

            }
            else
            {
                i = (int)Math.Round((p.Points[5].Y - Y) / d - 1);
                j = (int)Math.Round((p.Points[5].X - X) / d);
            }

            if(k == 0)
            {
                if(i < C2)
                {
                    Col[i][j]++;
                    if(Col[i][j] == 4)
                    {
                        if (player == 0)
                        {
                            R[i][j].Fill = Brushes.Blue;
                            b++;
                            sc++;
                        }
                        else
                        {
                            R[i][j].Fill = Brushes.Red;
                            r++;
                            sc++;
                        }
                        w = true;
                        
                    }
                }
                if (i > 0)
                {
                    Col[i - 1][j]++;
                    if (Col[i - 1][j] == 4)
                    {
                        if (player == 0)
                        {
                            R[i - 1][j].Fill = Brushes.Blue;
                            b++;
                            sc++;
                        }
                        else
                        {
                            R[i - 1][j].Fill = Brushes.Red;
                            r++;
                            sc++;
                        }
                        w = true;
                    }
                }
            }
            else
            {
                if (j > 0)
                {
                    Col[i][j - 1]++;
                    if (Col[i][j - 1] == 4)
                    {
                        if (player == 0)
                        {
                            R[i][j-1].Fill = Brushes.Blue;
                            b++;
                            sc++;
                        }
                        else
                        {
                            R[i][j-1].Fill = Brushes.Red;
                            r++;
                            sc++;
                        }
                        w = true;
                    }
                }
                if (j < C1)
                {
                    Col[i][j]++;
                    if (Col[i][j] == 4)
                    {
                        if (player == 0)
                        {
                            R[i][j].Fill = Brushes.Blue;
                            b++;
                            sc++;
                        }
                        else
                        {
                            R[i][j].Fill = Brushes.Red;
                            r++;
                            sc++;
                        }
                        w = true;
                    }
                }
            }

            if (w == false)
            {
                player = (player + 1) % 2;
            }

            p.IsEnabled = false;

            if (sc == C1 * C2)
            {
                if (b > r)
                {

                    MessageBoxResult result = MessageBox.Show("Победил первый игрок (Синий). Начать новую игру ?",
                  "Failed", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                        Reset();
                    else
                        Close();
                }
                else
                {
                    if (r > b)
                    {

                        MessageBoxResult result = MessageBox.Show("Победил второй игрок (Красный). Начать новую игру ?",
                     "Failed", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                            Reset();
                        else
                            Close();

                    }
                    else
                    {

                        MessageBoxResult result = MessageBox.Show("Победила дружба. Начать новую игру ?",
                     "Failed", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                            Reset();
                        else
                            Close();
                    }
                }
            }

            
        }

        private void Reset()
        {
            for(int i = 0; i< C1; i++)
            {
                for(int j = 0; j < C2 + 1; j++)
                {
                    for(int k = 0; k< 2; k++)
                    {
                        P[i][j][k].Fill = Brushes.White;
                        P[i][j][k].IsEnabled = true;
                    }
                }
            }

            for(int i = 0; i< C1; i++)
            {
                for(int j = 0; j < C2; j++)
                {
                    R[i][j].Fill = grid1.Background;
                    Col[i][j] = 0;
                }
            }

            b = r = sc = 0;
        }
    }
}
