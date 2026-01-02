using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace lab6
{
    public class Piano : UserControl
    {
        private Color naturalNotesUpColor = Color.White;
        private Color naturalNotesDownColor = Color.Gray;
        private Color pressedNaturalUpColor = Color.Red;
        private Color pressedNaturalDownColor = Color.DarkRed;

        private Color sharpNotesUpColor = Color.Gray;
        private Color sharpNotesDownColor = Color.Black;
        private Color pressedSharpUpColor = Color.Red;
        private Color pressedSharpDownColor = Color.DarkRed;
        private Pen pen = new Pen(Brushes.Black, 2);
        private int octaves = 1;
        private List<Octave> piano = new List<Octave>();
        private List<List<List<RectangleF>>> pianoList;
        public event EventHandler<string> NoteClicked;
        string pressedButton;
        
        public Color NaturalNotesUpColor
        {
            get { return naturalNotesUpColor; }
            set { naturalNotesUpColor = value; this.Invalidate(); }
        }
        public Color NaturalNotesDownColor
        {
            get { return naturalNotesDownColor; }
            set { naturalNotesDownColor = value; this.Invalidate(); }
        }
        public Color PressedNaturalUpColor
        {
            get { return pressedNaturalUpColor; }
            set { pressedNaturalUpColor = value; this.Invalidate(); }
        }
        public Color PressedNaturalDownColor
        {
            get { return pressedNaturalDownColor; }
            set { pressedNaturalDownColor = value; this.Invalidate(); }
        }
        public Color SharpNotesUpColor
        {
            get { return sharpNotesUpColor; }
            set { sharpNotesUpColor = value; this.Invalidate(); }
        }
        public Color SharpNotesDownColor
        {
            get { return sharpNotesDownColor; }
            set { sharpNotesDownColor = value; this.Invalidate(); }
        }
        public Color PressedSharpUpColor
        {
            get { return pressedSharpUpColor; }
            set { pressedSharpUpColor = value; this.Invalidate(); }
        }
        public Color PressedSharpDownColor
        {
            get { return pressedSharpDownColor; }
            set { pressedSharpDownColor = value; this.Invalidate(); }
        }
        public int Octaves
        {
            get { return octaves; }
            set
            {
                if (octaves < 1) octaves = 1;
                octaves = value;
                this.Update_piano();
                this.Invalidate();
            }
        }


        public Piano()
        {
            this.octaves = 1;
            this.pressedButton = "";
            this.Update_piano();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.UserPaint |
                  ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }
        public void Update_piano()
        {
            piano.Clear();
            pianoList = new List<List<List<RectangleF>>>();
            SizeF octaveSize = new Size((int)(this.Size.Width / octaves), this.Size.Height);
            PointF nullLocation = new PointF(0, 0);
            for (int i = 0; i < octaves; i++)
            {
                piano.Add(new Octave(new PointF(octaveSize.Width * i, 0), octaveSize, i));
                pianoList.Add(piano[i].ToList());
            }
        }
        public string IsClick(PointF location)
        {
            string str;
            for (int i = 0; i < piano.Count; i++)
            {
                str = piano[i].GetClicked(location);
                if (str != "")
                {
                    return str;
                }
            }
            return "";
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Update_piano();
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            for (int i = 0; i < pianoList.Count; i++)
            {
                for (int j = 0; j < pianoList[i][0].Count(); j++)
                {

                    e.Graphics.DrawRectangle(pen, pianoList[i][0][j].X, pianoList[i][0][j].Y, pianoList[i][0][j].Width, pianoList[i][0][j].Height);
                    if (piano[i].GetNaturalNoteName(j) == pressedButton)
                    {
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            pianoList[i][0][j],
                            pressedNaturalUpColor,
                            pressedNaturalDownColor,
                            LinearGradientMode.Vertical))
                        {

                            e.Graphics.FillRectangle(brush, pianoList[i][0][j]);
                        }
                    ;
                    }
                    else
                    {
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            pianoList[i][0][j],
                            naturalNotesUpColor,
                            naturalNotesDownColor,
                            LinearGradientMode.Vertical))
                        {

                            e.Graphics.FillRectangle(brush, pianoList[i][0][j]);
                        };
                    }
                }
                for (int j = 0; j < pianoList[i][1].Count(); j++)
                {
                    e.Graphics.DrawRectangle(pen, pianoList[i][1][j].X, pianoList[i][1][j].Y, pianoList[i][1][j].Width, pianoList[i][1][j].Height);
                    if (piano[i].GetSharpNoteName(j) == pressedButton)
                    {
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            pianoList[i][1][j],
                            pressedSharpUpColor,
                            pressedSharpDownColor,
                            LinearGradientMode.Vertical))
                        {

                            e.Graphics.FillRectangle(brush, pianoList[i][1][j]);
                        }
                    ;
                    }
                    else
                    {
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                            pianoList[i][1][j],
                            sharpNotesUpColor,
                            sharpNotesDownColor,
                            LinearGradientMode.Vertical))
                        {
                            e.Graphics.FillRectangle(brush, pianoList[i][1][j]);
                        }
                    ;
                    }
                    
                }
                e.Graphics.DrawRectangle(pen, 0, 0, this.Size.Width, this.Size.Height);
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (piano == null) return;

            string clickedNote = ""; // если ничего не нажато

            // проверяем чёрные клавиши сначала (они сверху)
            foreach (var octave in piano)
            {
                string note = octave.GetClicked(e.Location);
                if (note != "")
                {
                    clickedNote = note;
                    pressedButton = note;
                    Invalidate();
                    break;
                }
            }

            if (clickedNote != "")
            {
                // вызываем событие
                NoteClicked?.Invoke(this, clickedNote);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            pressedButton = "";
            Invalidate();
        }
    }
}
