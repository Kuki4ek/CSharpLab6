using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    internal class Octave
    {
        public List<Note> naturalNotes;
        public List<Note> sharpNotes;
        public int naturalNotesCount = 7;
        public int sharpNotesCount = 5;
        public int number;
        public SizeF naturalNoteSize;
        public SizeF sharpNoteSize;
        public PointF location;
        public SizeF size;
        public Octave(PointF location, SizeF size, int number)
        {
            this.number = number;
            this.location = location;
            this.size = size;
            this.Update(location, size);
        }
        public void Update(PointF location, SizeF size)
        {
            this.location = location;
            this.size = size;
            naturalNoteSize = new SizeF((size.Width / naturalNotesCount), size.Height);
            sharpNoteSize = new SizeF(naturalNoteSize.Width * 0.4f, naturalNoteSize.Height * 0.6f);
            naturalNotes = new List<Note>();
            sharpNotes = new List<Note>();

            string[] notesNameStr = {"C", "D", "E", "F", "G", "A", "B"};
            for (int i = 0; i < naturalNotesCount; i++)
            {
                Note note = new Note();
                PointF noteLocation;
                if (i == 0)
                {
                    noteLocation = location;
                }
                else
                {
                    noteLocation = new PointF(i * naturalNoteSize.Width + location.X, 0 + location.Y);
                }
                RectangleF r = new RectangleF(noteLocation, naturalNoteSize);
                note.IsSharp = false;
                note.Name = $"{notesNameStr[i]}{number}";
                note.Rect = r;
                naturalNotes.Add(note);
            }
            for (int i = 0; i < sharpNotesCount; i++)
            {
                Note note = new Note();
                PointF noteLocation;
                if (i < 2)
                {
                    noteLocation = new PointF(naturalNotes[i+1].Rect.Location.X - (sharpNoteSize.Width / 2), naturalNotes[i + 1].Rect.Location.Y);
                    note.Name = $"{notesNameStr[i]}#{number}";
                }
                else
                {
                    noteLocation = new PointF(naturalNotes[i + 2].Rect.Location.X - (sharpNoteSize.Width / 2), naturalNotes[i + 1].Rect.Location.Y);
                    note.Name = $"{notesNameStr[i+1]}#{number}";
                }
                RectangleF r = new RectangleF(noteLocation, sharpNoteSize);
                note.IsSharp = true;
                note.Rect = r;
                sharpNotes.Add(note);
            }
        }
        public string GetClicked(PointF location)
        {
            for (int i = 0; i < sharpNotesCount; i++)
            {
                if (sharpNotes[i].IsClick(location))
                {
                    return sharpNotes[i].Name;
                }
                
            }
            for (int i = 0; i < naturalNotesCount; i++)
            {
                if (naturalNotes[i].IsClick(location))
                {
                    return naturalNotes[i].Name;
                }
            }
            return "";
        }
        public List<List<RectangleF>> ToList()
        {
            List<List<RectangleF>> octave = new List<List<RectangleF>>();
            List<RectangleF> naturalNotes = new List<RectangleF>();
            List<RectangleF> sharpNotes = new List<RectangleF>();
            for (int i = 0; i < naturalNotesCount; i++)
            {
                naturalNotes.Add(this.naturalNotes[i].GetRect());
            }
            for (int i = 0; i < sharpNotesCount; i++)
            {
                sharpNotes.Add(this.sharpNotes[i].GetRect());
            }
            octave.Add(naturalNotes);
            octave.Add(sharpNotes);
            return octave;
        }
        public string GetNaturalNoteName(int index)
        {
            if (index < 0 || index >= naturalNotesCount) return "";
            return naturalNotes[index].Name;
        }
        public string GetSharpNoteName(int index)
        {
            if (index < 0 || index >= sharpNotesCount) return "";
            return sharpNotes[index].Name;
        }
        public RectangleF GetNoteRectangle(string noteName)
        {
            RectangleF rectangle = new RectangleF(0f, 0f, 0f, 0f);
            for (int i = 0; i < naturalNotesCount; i++)
            {
                if (naturalNotes[i].Name == noteName)
                {
                    rectangle = naturalNotes[i].Rect;
                }
            }
            for (int i = 0; i < sharpNotesCount; i++)
            {
                if (sharpNotes[i].Name == noteName)
                {
                    rectangle = sharpNotes[i].Rect;
                }
            }
            return rectangle;
        }
    }
}
