namespace Seraphine.Helpers
{
    public static class FileHelper
    {
        public static string SalvarArquivoBase64(string base64, string nomeArquivo, string pastaDestino)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return string.Empty;

            if (!Directory.Exists(pastaDestino))
            {
                Directory.CreateDirectory(pastaDestino);
            }
            
            if (File.Exists(Path.Combine(pastaDestino, nomeArquivo)))
                File.Delete(Path.Combine(pastaDestino, nomeArquivo));

            var prefixo = base64.Split(',');
            string caminhoArquivo = Path.Combine(pastaDestino, nomeArquivo);
            byte[] fileBytes = Convert.FromBase64String(prefixo.Last());
            File.WriteAllBytes(caminhoArquivo, fileBytes);

            return caminhoArquivo;
        }

        public static string SalvarObterFilePath(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            var filePath = Path.Combine(directoryPath, fileName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (!fileName.EndsWith(".csv"))
            {
                return string.Empty;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return filePath;
        }

        public static List<string> BuscarLinhasArquivo(string filePath, Func<string, bool> validarCabecalho)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return [];
            }

            var linhas = new List<string>();
            using (var reader = new StreamReader(File.OpenRead(filePath)))
            {
                var cabecalho = reader.ReadLine();
                if (cabecalho == null || !validarCabecalho(cabecalho))
                {
                    reader.Close();
                    File.Delete(filePath);
                    return [];
                }

                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(linha))
                    {
                        linhas.Add(linha);
                    }
                }
            }

            return linhas;
        }

        public static void DeletarArquivo(string filePath)
        {
            File.Delete(filePath);
        }
    }
}