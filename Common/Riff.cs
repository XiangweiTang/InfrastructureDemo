using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    public abstract class Riff
    {
        protected bool DeepFlag = true;
        public int ShallowThreshold { get; set; } = 100;
        protected byte[] RiffBytes = new byte[0];
        protected int RiffLength = 0;
        protected int CurrentIndex = 0;
        protected List<RiffChunk> ChunkList = new List<RiffChunk>();
        public Riff() { }
        public void DeepParse(string filePath)
        {
            DeepFlag = true;
            RiffBytes = File.ReadAllBytes(filePath);
            ParseRiffHeader();
            ParseRiff();
            PostCheck();
        }

        public void ShallowParse(string filePath)
        {
            DeepFlag = false;
            RiffBytes = IO.ReadBytes(filePath, ShallowThreshold);
            ParseRiffHeader();
            ParseRiff();
            PostCheck();
        }

        private void ParseRiffHeader()
        {
            Sanity.Requires(RiffBytes.Length >= 12, $"Riff header broken, file length is {RiffBytes.Length}.");
            Sanity.Requires(GetChunkName(0) == "RIFF", "Riff header broken, not RIFF file.");
            RiffLength = BitConverter.ToInt32(RiffBytes, 4);
            // If !DeepFlag, we don't check the length.
            // Otherwise, we check the length.
            Sanity.Requires(!DeepFlag || RiffBytes.Length == RiffLength + 8, $"Riff file broken, expected length is {RiffLength + 8}, actual length is {RiffBytes.Length}.");
        }

        protected abstract void ParseRiff();
        protected abstract void PostCheck();
        protected string GetChunkName(int offset)
        {
            return Encoding.ASCII.GetString(RiffBytes, offset, 4);
        }
        protected int GetChunkSize(int offset)
        {
            return BitConverter.ToInt32(RiffBytes, offset);
        }
    }
    public struct RiffChunk
    {
        public string Name;
        public int Offset;
        public int Size;
    }
}
