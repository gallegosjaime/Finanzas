using Finanzas.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace Finanzas.Services
{
    class FirebaseHelper
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://finanzas-8e52d-default-rtdb.firebaseio.com/");

        public static async Task<bool> AgregarIngresos(int cantidad, String descripcion)
        {
            //Await = Se ejecuta completamente antes de continuar con el resto del código, es obligatorio ponerlo
            //Child = EL hijo o nombre de la Tabla a la que se guardarán los datos
            //new MontoModel es el objeto que vamos a enviar y es el model que tiene las varibles

            //Try -Catch es por si sucede algún error
            try
            {
                //Post async = Enviar datos a la BD
                //paso 2: Enviar a firebase
                await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").PostAsync(new MontoModel()
                {
                    // Variable del MontoModel (dichas variables fueron creadas en esa clase que está en la carpeta models)
                    //   |
                    //   ↓        ↓ Variables que están en el paréntesis del método y reciben los datos desde donde se llame
                    nombreAccion = "Ingreso",
                    cantidad = cantidad,
                    descripcion = descripcion,
                    registroID = Guid.NewGuid(),
                    fechaSubida = DateTime.Now.ToString(),
                    fechaUpdate = "-"
                });
                await getHistorialIngresos();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<List<MontoModel>> getHistorialIngresos()
        {
            try
            {
                //Recibir los datos en forma de lista
                return (await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").OnceAsync<MontoModel>()).Select(item => new MontoModel 
                {
                    nombreAccion = item.Object.nombreAccion,
                    registroID = item.Object.registroID, 
                    fechaSubida = item.Object.fechaSubida,
                    fechaUpdate = item.Object.fechaUpdate,
                    cantidad= item.Object.cantidad,
                    descripcion= item.Object.descripcion
                }).ToList();                
            }
            catch
            {
                return null;
            }
        }
        public static async Task<bool> updateRegistro(Guid id, int cantidad,String descripcion)
        {
            try
            {
                //Está en posibilidad de que este código cambie para hacerlo similar a los anteriores y en vez de tener
                //un ID generado por nosotros, utilicemos el ID autogenerado por firebase.

                //Localizar el "ID" autogenerado por Firebase donde se encuentra dicho registro en base al ID
                var toUpdateStatus = (await firebaseClient.Child("Usuarios").Child("ID")
                    .Child("Historial").OnceAsync<MontoModel>()).Where(a => a.Object.registroID == id).FirstOrDefault();
                
                //Asignar los valores actualizados
                toUpdateStatus.Object.cantidad = cantidad;
                toUpdateStatus.Object.descripcion = descripcion;
                toUpdateStatus.Object.fechaUpdate = DateTime.Now.ToString();
                //Mandar la actualización
                await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").Child(toUpdateStatus.Key).PutAsync(toUpdateStatus.Object);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> deleteRegistro(Guid id)
        {
            try
            {
                //Obtener el key(ID hecho por firebase) en base al ID del registro
                var toUpdateStatus = (await firebaseClient.Child("Usuarios").Child("ID")
                    .Child("Historial").OnceAsync<MontoModel>()).Where(a => a.Object.registroID == id).FirstOrDefault();
                //Mandar la operación del delete a firebase
                await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").Child(toUpdateStatus.Key).DeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
