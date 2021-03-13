using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GroupPoster.Infrastructure.DataAccess.DataInteractor
{
    public class FileInteractor
    {
        private readonly SemaphoreSlim gate;
        private const string SaveFolder = "./Saves";

        public FileInteractor(SemaphoreSlim gate)
        {
            Directory.CreateDirectory("./Saves");
            this.gate = gate;
        }

        public async Task<(bool, string)> Read(string fileName)
        {
            await gate.WaitAsync().ConfigureAwait(false);

            string path = GetPath(fileName);
            var output = File.Exists(path) ? (true, File.ReadAllText(path)) : (false, string.Empty);

            gate.Release();
            
            return output;
        }

        public async Task Write(string fileName, string data)
        {
            await gate.WaitAsync().ConfigureAwait(false);

            string path = GetPath(fileName);
            File.WriteAllText(path, data);

            gate.Release();
        }

        private string GetPath(string fileName)
        {
            return Path.Combine(SaveFolder, fileName);
        }
    }
}
