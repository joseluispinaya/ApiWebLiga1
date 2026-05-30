using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace ApiWebLiga.Helpers
{
    public class Utilidadesj
    {
        public static string UploadPhotoToCloud(MemoryStream stream, string fileName)
        {
            string imageUrl = "";

            try
            {
                // 1. Configurar las credenciales (Lo ideal es leer esto del Web.config)
                Account account = new Account(
                    "TU_CLOUD_NAME",
                    "TU_API_KEY",
                    "TU_API_SECRET"
                );

                Cloudinary cloudinary = new Cloudinary(account);

                // 2. Asegurar que el stream esté al inicio
                stream.Position = 0;

                // 3. Configurar los parámetros de subida
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileName, stream),
                    Folder = "FutsalRiberalta/Logos", // Crea carpetas ordenadas en tu nube
                    Overwrite = true
                };

                // 4. Ejecutar la subida
                var uploadResult = cloudinary.Upload(uploadParams);

                // 5. Verificar si fue exitoso
                if (uploadResult.StatusCode == HttpStatusCode.OK)
                {
                    // Devolvemos la URL segura (https) generada por Cloudinary
                    imageUrl = uploadResult.SecureUrl.ToString();
                }
            }
            catch (Exception)
            {
                // Opcional: Loguear el error
                imageUrl = "";
            }

            return imageUrl;
        }

        public static string UploadPhoto(MemoryStream stream, string folder)
        {
            string rutaa = "";

            try
            {
                stream.Position = 0;

                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";

                var fullPath = $"{folder}{file}";
                var path = Path.Combine(HttpContext.Current.Server.MapPath(folder), file);

                // Guardar la imagen en el sistema de archivos
                File.WriteAllBytes(path, stream.ToArray());

                // Verificar si el archivo fue guardado correctamente
                if (File.Exists(path))
                {
                    rutaa = fullPath;
                }
            }
            catch (IOException)
            {
                rutaa = "";
            }
            catch (Exception)
            {
                rutaa = "";
            }
            return rutaa;
        }
    }
}