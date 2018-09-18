using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Accord.Video.FFMPEG;


namespace dawu {
    public partial class Form1 : Form {

        Dictionary<string, int> dIChing = new Dictionary<string, int>() {
            { "111111",1 },
            {"000000", 2},
            {"010001", 3},
            {"100010", 4},
            {"010111", 5},
            {"111010", 6},
            {"000010", 7},
            {"010000", 8},
            {"110111", 9},
            {"111011", 10},
            {"000111", 11},
            {"111000", 12},
            {"111101", 13},
            {"101111", 14},
            {"000100", 15},
            {"001000", 16},
            {"011001", 17},
            {"100110", 18},
            {"000011", 19},
            {"110000", 20},
            {"101001", 21},
            {"100101", 22},
            {"100000", 23},
            {"000001", 24},
            {"111001", 25},
            {"100111", 26},
            {"100001", 27},
            {"011110", 28},
            {"010010", 29},
            {"101101", 30},
            {"011100", 31},
            {"001110", 32},
            {"111100", 33},
            {"001111", 34},
            {"101000", 35},
            {"000101", 36},
            {"110101", 37},
            {"101011", 38},
            {"010100", 39},
            {"001010", 40},
            {"100011", 41},
            {"110001", 42},
            {"011111", 43},
            {"111110", 44},
            {"011000", 45},
            {"000110", 46},
            {"011010", 47},
            {"010110", 48},
            {"011101", 49},
            {"101110", 50},
            {"001001", 51},
            {"110100", 53},
            {"100100", 52},
            {"001011", 54},
            {"001101", 55},
            {"101100", 56},
            {"110110", 57},
            {"011011", 58},
            {"110010", 59},
            {"010011", 60},
            {"110011", 61},
            {"001100", 62},
            {"010101", 63},
            {"101010", 64 }
        };

        public Form1() {
            InitializeComponent();
        }

        char iChing2char(List<string> line) {
            int x = 0;
            foreach (var s in line) {
                x += dIChing[s];
            }
            return (char)x;
        }

        private void btnRun_Click(object sender, EventArgs e) {
            VideoFileReader vfr = new VideoFileReader();
            if (!File.Exists(txtInFile.Text)) {
                MessageBox.Show("The file in the Vidya File box does not exist. Please type a path to an .mp4 file in the Vidya File box then click run.");
                return;
            }
            vfr.Open(txtInFile.Text);
            List<char> res = new List<char>();
            for (int i = 0; i < vfr.FrameCount; i++) {
                Application.DoEvents();
                Bitmap b = vfr.ReadVideoFrame(i);
                Pen green = new Pen(Color.Green), red = new Pen(Color.Red);
                Graphics g = Graphics.FromImage(b);
                int ys = 180;
                pictureBox1.Image = b;
                int h = 24, w = 18;
                List<List<System.Drawing.Point>> lines = new List<List<System.Drawing.Point>>();
                for (int j = ys + 3; j < b.Height; j++) {
                    int x = 0;
                    for (int k = 0; k < b.Width; k++) {
                        Color c = b.GetPixel(k, j);
                        if (c.R < 127 && c.B < 127 && c.G < 127) x++;
                    }
                    if (x > 10) {
                        lines.Add(new List<System.Drawing.Point>() {
                            new System.Drawing.Point(0,j),
                            new System.Drawing.Point(b.Width, j)
                        });
                        lines.Add(new List<System.Drawing.Point>() {
                            new System.Drawing.Point(0, j + h),
                            new System.Drawing.Point(b.Width, j + h)
                        });
                        List<string> line = new List<string>();
                        List<int> starts = new List<int>();
                        for (int a = 0; a < b.Width; a++) {
                            int z = 0;
                            for (int c = j; c <= j + h / 3 && b.Height > j + h; c++) {
                                Color d = b.GetPixel(a, c);
                                if (d.R < 127 && d.B < 127 && d.G < 127) z++;
                            }
                            if (z > 2) {
                                starts.Add(a);
                                lines.Add(new List<System.Drawing.Point>() {
                                    new System.Drawing.Point(a,j),
                                    new System.Drawing.Point(a, j+h)
                                });
                                lines.Add(new List<System.Drawing.Point>() {
                                    new System.Drawing.Point(a+w,j),
                                    new System.Drawing.Point(a+w, j+h)
                                });
                                int[] avg = new int[h];

                                for (int m = j; m < j + h; m++) {
                                    avg[m - j] = 0;
                                    for (int n = a + w / 3; n < a + 2 * w / 3; n++) {
                                        Color dat = b.GetPixel(n, m);
                                        avg[m - j] += dat.R + dat.G + dat.B;
                                    }
                                }

                                int[] bin = new int[6];
                                int s = h / 6;
                                for (int t = 0; t < h; t++) {
                                    bin[t / s] += avg[t] / (w / 3) / s / 3;
                                }
                                string bs = "";
                                for (int t = 0; t < bin.Length; t++) {
                                    bs += bin[t] < 200 ? "1" : "0";
                                }
                                line.Add(bs);
                                a += w * 3;
                            }
                        }
                        res.Add(iChing2char(line));
                        j += h;                    
                    }
                }
                foreach (var line in lines) {
                    g.DrawLine(red, line[0].X, line[0].Y, line[1].X, line[1].Y);
                }
                pictureBox1.Refresh();
                Application.DoEvents();
                textBox1.Text += string.Join("\r\n", string.Join("", res.ToArray()));
                Application.DoEvents();
            }
        }
    }
}
