namespace PsMidiProfiler
{
    public interface INoteHighlighter
    {
        void HighlightNote(byte note, bool isNoteOn, byte velocity);
    }
}
