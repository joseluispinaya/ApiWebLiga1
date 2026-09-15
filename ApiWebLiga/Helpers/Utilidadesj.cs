using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
                    "xxxxx",
                    "xxxxxx",
                    "xxxxxx"
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

        public static bool Verify(string password, string hash)
        {
            // Validamos que ninguno de los dos sea nulo o vacío
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
                return false;

            // Verifica si la contraseña en texto plano coincide con el hash de la BD
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public static string Hash(string password)
        {
            // Validamos que no nos envíen contraseñas vacías
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            // Encripta la contraseña. BCrypt genera y aplica el "Salt" automáticamente
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool EnviosCorreos(string toEmailUser, string subjec, string nombreUser, string confirmUrl)
        {
            try
            {
                var from = "chavesmendez2@gmail.com";
                var name = "Soporte Futsal Riberalta";
                var smtps = "smtp.gmail.com";
                var port = 587;
                var password = "xxxxxxxx";

                var correo = new MailMessage
                {
                    From = new MailAddress(from, name)
                };
                correo.To.Add(toEmailUser);
                correo.Subject = subjec;

                // Plantilla HTML Premium - Adaptada a la identidad de la App
                string cuerpo = $@"
                    <!DOCTYPE html>
                    <html lang='es'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Recuperar Contraseña</title>
                    </head>
                    <body style='margin: 0; padding: 0; background-color: #f3f4f6; font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif;'>
                        <table width='100%' border='0' cellspacing='0' cellpadding='0' style='background-color: #f3f4f6; padding: 40px 10px;'>
                            <tr>
                                <td align='center'>
                                    <!-- Contenedor Principal -->
                                    <table width='600' border='0' cellspacing='0' cellpadding='0' style='background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.05); max-width: 600px; width: 100%;'>
                                
                                        <!-- Cabecera Oscura con Borde Rojo -->
                                        <tr>
                                            <td align='center' style='background-color: #0a0e17; padding: 35px 20px; border-bottom: 4px solid #e60000;'>
                                                <h1 style='color: #ffffff; margin: 0; font-size: 26px; letter-spacing: 1px; text-transform: uppercase;'>A.M.F.S.R</h1>
                                                <p style='color: #9ca3af; font-size: 14px; font-style: italic; margin: 5px 0 0 0;'>Pasión por el Futsal</p>
                                            </td>
                                        </tr>
                    
                                        <!-- Cuerpo del Correo -->
                                        <tr>
                                            <td style='padding: 40px 30px; color: #374151;'>
                                                <p style='margin-top: 0; font-size: 18px; color: #111827;'>Hola <strong>{nombreUser}</strong>,</p>
                                                <p style='font-size: 15px; line-height: 1.6; color: #4b5563; margin-bottom: 30px;'>
                                                    Hemos recibido una solicitud para restablecer la contraseña de tu cuenta. Si fuiste tú, haz clic en el botón de abajo para crear una nueva clave y recuperar tu acceso al sistema.
                                                </p>
                            
                                                <!-- Botón de Acción Rojo -->
                                                <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                                                    <tr>
                                                        <td align='center'>
                                                            <a href='{confirmUrl}' style='background-color: #e60000; color: #ffffff; text-decoration: none; padding: 14px 35px; border-radius: 8px; font-size: 16px; font-weight: bold; display: inline-block; text-transform: uppercase; letter-spacing: 0.5px;'>Restablecer Contraseña</a>
                                                        </td>
                                                    </tr>
                                                </table>
                            
                                                <!-- Enlace de respaldo -->
                                                <p style='font-size: 13px; line-height: 1.5; color: #6b7280; margin-top: 35px; margin-bottom: 0;'>
                                                    Si el botón no funciona, copia y pega el siguiente enlace en tu navegador web:<br>
                                                    <a href='{confirmUrl}' style='color: #0044ff; word-break: break-all; text-decoration: underline;'>{confirmUrl}</a>
                                                </p>
                            
                                                <!-- Advertencia de Seguridad -->
                                                <p style='font-size: 13px; line-height: 1.5; color: #9ca3af; margin-top: 30px; border-top: 1px solid #e5e7eb; padding-top: 20px;'>
                                                    Si no realizaste esta solicitud, ignora este mensaje. Tu cuenta está segura y tu contraseña actual seguirá siendo válida.
                                                </p>
                                            </td>
                                        </tr>
                    
                                        <!-- Pie de página -->
                                        <tr>
                                            <td align='center' style='background-color: #f9fafb; padding: 20px; border-top: 1px solid #e5e7eb;'>
                                                <p style='margin: 0; font-size: 12px; color: #9ca3af; line-height: 1.5;'>
                                                    &copy; {DateTime.Now.Year} Asociación Municipal de Futsal de Riberalta.<br>Todos los derechos reservados.
                                                </p>
                                            </td>
                                        </tr>
                    
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>";

                correo.Body = cuerpo;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;

                SmtpClient smtp = new SmtpClient
                {
                    Host = smtps,
                    Port = port,
                    Credentials = new NetworkCredential(from, password),
                    EnableSsl = true
                };

                smtp.Send(correo);
                return true;
            }
            catch (SmtpException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
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