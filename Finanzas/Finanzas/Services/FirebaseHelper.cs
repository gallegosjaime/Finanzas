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

        public static async Task<bool> AgregarIngresos(int cantidad, String descripcion, string nombreAccion)
        {
            //El código comienza desde Ingresos.CS y próximamente gastos

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
                    nombreAccion = nombreAccion,
                    descripcion = descripcion
                });

                // ACTUALIZAR PRESUPUESTO

                //Validar que exista la tabla "presupuesto" 
                if((await firebaseClient.Child("Usuarios").Child("ID").Child("Presupuesto").OnceAsync<MontoModel>()).FirstOrDefault() == null){
                    //Si no existe la va a crear
                    await firebaseClient.Child("Usuarios").Child("ID").Child("Presupuesto").PostAsync(new MontoModel()
                    {
                        cantidad = cantidad,
                    });
                }
                else
                {
                    //Si existe la tabla, entonces va a obtener la cantidad actual
                    var cantidadAnterior = await getPresupuesto();
                    //Validar si la acción es un ingreso
                    if (nombreAccion.ToLower() == "ingreso")
                    {
                        //Actualizará la nueva cantidad sumandole la nueva
                        await firebaseClient.Child("Usuarios").Child("ID").Child("Presupuesto").Child(cantidadAnterior.ID).PutAsync(new MontoModel()
                        {
                            cantidad = cantidad + cantidadAnterior.cantidad,
                        });
                    }
                    else 
                    { 
                        //Código de gasstos
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<MontoModel> getPresupuesto()
        {
            //El código comienza desde AboutPage.cs

            //Hacer la petición para traer todos los datos de la tabla
            var data = (await firebaseClient.Child("Usuarios").Child("ID").Child("Presupuesto").OnceAsync<MontoModel>()).FirstOrDefault();
            //EN caso de que no exista la tabla prespuesto estos valores serán los de por defecto
            var cantidad = 0;
            var ID = "";
               
            //Validar que si existen datos entonces se guardarán en esas variables para reemplazar a los de por defecto
            if (data != null)
            {
                cantidad = data.Object.cantidad;
                ID = data.Key;
            }

            //Se crea un objeto para almacenar los datos
            MontoModel montoModel = new MontoModel()
            {
                cantidad = cantidad,
                ID = ID
            };
            //Se devuelve el objeto para después utilizarlo
            return montoModel;
        }


        public static async Task<List<MontoModel>> getHistorial()
        {
            //El código comienza desde Historial.cs

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
        public static async Task<bool> updateRegistro(string id, int Nuevacantidad, string descripcion, string nombreAccion)
        {
            //El código comienza desde EditarHistorial.cs
            try
            {
                //Buscar el registro en base al ID autogenerado por firebase
                var updateRegistro = (await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").OnceAsync<MontoModel>()).Where(a => a.Key == id).FirstOrDefault();

                //Guardar la cantidad original de este registro en la siguiente variable para utilizarla cuando se actualice el prespuesto
                var cantidadAnterior = updateRegistro.Object.cantidad;

                //Actualizar los campos del registro

                updateRegistro.Object.cantidad = Nuevacantidad;     
                updateRegistro.Object.descripcion = descripcion;      
                updateRegistro.Object.fechaUpdate = DateTime.Now.ToString();     

                //Mandar los nuevos datos del registro a la tabla Historial en firebase
                await firebaseClient.Child("Usuarios").Child("ID").Child("Historial").Child(updateRegistro.Key).PutAsync(updateRegistro.Object);


                //ACTUALIZAR PRESUPUESTO

                //Obtener el registro del presupuesto total junto con su key autogenerado por firebase
                var tablaPresupuesto = await getPresupuesto();

                //Sacar la diferencia entre la cantidad original que tenía el registro de la tabla Historial y la nueva ingresada
                var diferencia = Nuevacantidad - cantidadAnterior;

                //Validar si es un ingreso lo que se modificó
                if (nombreAccion.ToLower() == "ingreso")
                {
                    //Reasignar la nueva cantidad
                    await firebaseClient.Child("Usuarios").Child("ID").Child("Presupuesto").Child(tablaPresupuesto.ID).PutAsync(new MontoModel()
                    {
                        //Sumar la diferencia con la cantidad total que tiene la tabla prespuesto
                        cantidad = tablaPresupuesto.cantidad+ diferencia,
                    });
                }
                else
                {
                    //Código de gastos
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> deleteRegistro(string id, int cantidad, String nombreAccion)
        {
            //El código comienza desde Historial.cs
            try
            {
                //ACTUALIZAR PRESUPUESTO 

                //Recibir el registro con el presupuesto total
                var cantidadAnterior = await getPresupuesto();
                //validar si estamos borrando uno de ingresos
                if (nombreAccion.ToLower() == "ingreso")
                {
                    //Reasignar la nueva cantidad
                    await firebaseClient.Child("Usuarios").Child("ID").Child("Presupuesto").Child(cantidadAnterior.ID).PutAsync(new MontoModel()
                    {
                        cantidad = cantidadAnterior.cantidad - cantidad,
                    });
                }
                else
                {
                    //CÓDIGO DE GASTOS
                }

                //BORRAR EL REGISRO EN LA TABLA HISTORIAL

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
