using Finanzas.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Finanzas.Services
{
    class FirebaseHelper
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://finanzas-8e52d-default-rtdb.firebaseio.com/");

        public static async Task<bool> AgregarIngresos(int cantidad, String descripcion)
        {
            //Try -Catch es por si sucede algún error
            try
            {
                //Await = Se ejecuta completamente antes de continuar con el resto del código, es obligatorio ponerlo
                //Child = EL hijo o nombre de la Tabla a la que se guardarán los datos
                //Post async = Enviar datos a la BD
                //new MontoModel es el objeto que vamos a enviar y es el model que tiene las varibles

                //paso 2: Enviar a firebase
                await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").PostAsync(new MontoModel()
                {
                    // Variable del MontoModel
                    //   |
                    //   ↓        ↓ Variables que reciben los valores de los campos (los que se envían desde el formulario)
                    cantidad = cantidad,
                    fechaSubida = DateTime.Now.ToString(),
                    fechaUpdate = "-",
                    nombreAccion = "Ingreso",
                    descripcion = descripcion
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public static async Task<List<MontoModel>> getHistorial()
        {
            try
            {
                //Obtener todos los registros de la colección Historial en forma de lista
                var data = (await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").OnceAsync<MontoModel>()).Select(item => new MontoModel()
                {
                    cantidad = item.Object.cantidad,
                    fechaSubida = item.Object.fechaSubida,
                    fechaUpdate = item.Object.fechaUpdate,
                    nombreAccion = item.Object.nombreAccion,
                    descripcion = item.Object.descripcion,
                    ID = item.Key,
                }).ToList();
                return data;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<bool> updateRegistro(string id, int cantidad, string descripcion)
        {
            try
            {
                //Buscar el registro en base al ID autogenerado por firebase
                var updateRegistro = (await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").OnceAsync<MontoModel>()).Where(a => a.Key == id).FirstOrDefault();
                //Actualizar los campos del registro
                updateRegistro.Object.cantidad = cantidad;
                updateRegistro.Object.descripcion = descripcion;
                updateRegistro.Object.fechaUpdate = DateTime.Now.ToString();
                //Mandar actualización a firebase
                await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").Child(updateRegistro.Key).PutAsync(updateRegistro.Object);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> deleteRegistro(string id)
        {
            try
            {
                //Mandar la petición de borrar a firebase en base al ID autogenerado por firebase
                await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").Child(id).DeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
