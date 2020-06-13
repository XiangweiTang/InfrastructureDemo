using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Common
{
    public class Wave : Riff
    {
        public Wave() : base()
        {
            TypeDict = IO.ReadEmbed("Common.Internal.Wave.WaveType.txt")
                .ToDictionary(x => int.Parse(x.Split('\t')[0]), x => x.Split('\t')[1]);
        }
        Dictionary<int, string> TypeDict = new Dictionary<int, string>();
        RiffChunk FormatChunk = new RiffChunk { Name = "NA" };
        RiffChunk DataChunk = new RiffChunk { Name = "NA" };
        public short WaveTypeId { get; private set; } = 0;
        public string WaveType { get; private set; } = "NA";
        public short NumChannels { get; private set; } = 0;
        public int SampleRate { get; private set; } = 0;
        public int ByteRate { get; private set; } = 0;
        public short BlockAlign { get; private set; } = 0;
        public short BitsPerSample { get; private set; } = 0;
        protected override void ParseRiff()
        {
            Sanity.Requires(RiffBytes.Length >= 44, $"Wave header broken, file length is {RiffBytes.Length}.");
            Sanity.Requires(GetChunkName(8) == "WAVE", "Wave header broken, not wave file.");
            ParseWave(12);
        }

        private void ParseWave(int offset)
        {
            // Reach the end of the file.
            if (offset == RiffBytes.Length)
                return;
            if (offset + 8 > RiffBytes.Length)
            {
                Sanity.Requires(!DeepFlag, "Wave file broken, file length is shorter than expected.");
                return;
            }
            RiffChunk chunk = new RiffChunk { Name = GetChunkName(offset), Size = GetChunkSize(offset + 4), Offset = offset };
            if (offset + 8 + chunk.Size > RiffBytes.Length)
            {
                Sanity.Requires(!DeepFlag, "Wave file broken, file length is shorter than expected.");
                return;
            }
            ChunkList.Add(chunk);
            ParseWave(offset + 8 + chunk.Size);
        }
        protected override void PostCheck()
        {
            CheckChunks();
        }

        private void CheckChunks()
        {
            foreach (var chunk in ChunkList)
            {
                switch (chunk.Name)
                {
                    case "fmt ":
                        Sanity.Requires(FormatChunk.Name == "NA", "More than one format chunk.");
                        FormatChunk = chunk;
                        break;
                    case "data":
                        Sanity.Requires(DataChunk.Name == "NA", "More than one data chunk.");
                        DataChunk = chunk;
                        break;
                    default:
                        break;
                }
            }

            CheckChunkFormat();
            CheckChunkData();
        }
        private void CheckChunkFormat()
        {
            Sanity.Requires(FormatChunk.Name == "fmt ", "Missing format chunk.");
            Sanity.Requires(FormatChunk.Size >= 16, $"Format chunk error, chunk size is {FormatChunk.Size}.");
            WaveTypeId = BitConverter.ToInt16(RiffBytes, FormatChunk.Offset + 8);
            NumChannels = BitConverter.ToInt16(RiffBytes, FormatChunk.Offset + 10);
            SampleRate = BitConverter.ToInt32(RiffBytes, FormatChunk.Offset+12);
            ByteRate = BitConverter.ToInt32(RiffBytes, FormatChunk.Offset + 16);
            BlockAlign = BitConverter.ToInt16(RiffBytes, FormatChunk.Offset + 20);
            BitsPerSample = BitConverter.ToInt16(RiffBytes, FormatChunk.Offset + 22);

            Sanity.Requires(WaveTypeId != 0, "Wave type id cannot be zero.");
            Sanity.Requires(NumChannels != 0, "Number of channel cannot be zero.");
            Sanity.Requires(SampleRate != 0, "Sample rate cannot be zero.");
            Sanity.Requires(ByteRate != 0, "Byte rate cannot be zero.");
            Sanity.Requires(BlockAlign != 0, "Block align cannot be zero.");
            Sanity.Requires(BitsPerSample != 0, "Bits per sample cannot be zero.");

            Sanity.Requires(ByteRate == SampleRate * NumChannels * BitsPerSample / 8, "Error in byte rate equation.");
            Sanity.Requires(BlockAlign == NumChannels * BitsPerSample / 8, "Error in block align equation.");

            if (TypeDict.ContainsKey(WaveTypeId))
                WaveType = TypeDict[WaveTypeId];
        }
        private void CheckChunkData()
        {
            Sanity.Requires(DataChunk.Name == "data", "Missing data chunk.");
        }        
    }
}
