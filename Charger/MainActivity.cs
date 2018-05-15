using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Java.Util;
using System;
using System.Collections.Generic;
using System.IO;
using Android.Views;

namespace Charger
{
    [Activity(Label = "Charger", MainLauncher = true)]
    public class MainActivity : Activity
    {
        //Adaptador del telefono
        private BluetoothAdapter BlueAdapter = null;

        //Coneccion con el arduino
        private BluetoothSocket btSocket = null;

        //Streams de lectura I/O
        private Stream outStream = null;

        //Pantalla de errores
        TextView Errores;

        //Boton de conexión
        ToggleButton Coneccion;

        //Botones de movimiento
        Button Advance;
        Button Regress;
        Button Left;
        Button Right;
        Button Stop;

        //Direccion del arduino unico dispositivo seleccinado por default
        private string address = "98:D3:32:31:2C:69";

        //Id Unico de comunicacion 
        private static UUID MY_UUID = UUID.FromString("00301101-0000-1001-8000-00805F9B34FB");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Errores = FindViewById<TextView>(Resource.Id.lbErrores);
            Coneccion = FindViewById<ToggleButton>(Resource.Id.btnConector);
            Advance = FindViewById<Button>(Resource.Id.btnAdvance);
            Regress = FindViewById<Button>(Resource.Id.btnRegress);
            Left = FindViewById<Button>(Resource.Id.btnLeft);
            Right = FindViewById<Button>(Resource.Id.btnRight);
            Stop = FindViewById<Button>(Resource.Id.btnStop);

            //Eventos de votones para movimiento
            Advance.Click += new EventHandler(this.Advance_Click);
            Regress.Click += new EventHandler(this.Regress_Click);
            Right.Click += new EventHandler(this.Right_Click);
            Left.Click += new EventHandler(this.Left_Click);
            Stop.Click += new EventHandler(this.Stop_Click);

            //evento de coneccion en tre el dispositivo y la maquina
            Coneccion.CheckedChange += new EventHandler<CompoundButton.CheckedChangeEventArgs>(this.Coneccion_Click);

            //Metodo de revicion de conexion bluetooth del dispositivo
            CheckBt();
        }


        //Coneccion con el robot
        private void Coneccion_Click(object sender, ToggleButton.CheckedChangeEventArgs e)
        {
            /*Al ser utilizado un ToggleButton tiene las opciones de activacion y de activacion,
              si el boton es activadoa el boton se inteta realizar la coneccion*/
            if (e.IsChecked)
            {
                //si se activa el toggle button se incial el metodo de conexion
                //si la coneccion es no es exitosa el boton se desactiva
                if (!Connect())
                {
                    Coneccion.PerformClick();
                }


            }
            else
            {
                //en caso de desactivar el toggle button se desconecta el dispositivo
                try
                {
                    /*si la existe alguna coneccion cuando el boton es desactivado el programa cierra 
                    la coneccion entre dispositivos*/
                    if (btSocket.IsConnected)
                    {
                        try
                        {
                            //cerramos la conexion
                            btSocket.Close();
                            //y el programa libera el bluetooth del dispositivo el cual retoma la busqueda de dispositivos para conctarse
                            BlueAdapter.StartDiscovery();
                        }
                        catch (Exception ex)
                        {
                            //si existe algun problema en el cierre de conexion  se mada el mensaje al usuario
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                //en dado caso de que no exista una coneccion en el programa se genera una excepcion
                //la cual es atrapada y simplemente se desactiva el boton
                catch (Exception)
                {

                }
            }
        }
        //Movimiento hacia a delante
        private void Advance_Click(object sender, EventArgs e)
        {
            //el comando try es utilizado cuando el dispositivo no esta conectado
            //puesto que para dicha comprobacion al existe la posibilidad de que aun no se haya realizado la conexion y no exista el soket 
            try
            {
                //si existe la conexxion manda el comando especificado
                if (btSocket.IsConnected)
                {
                    //para uso de este programa el comando de avance es la letra "W"
                    WriteData(new Java.Lang.String("W"));
                }

            }
            catch (Exception)
            {
            }
        }
        //Movimiento hacia atras
        private void Regress_Click(object sender, EventArgs e)
        {
            //el comando try es utilizado cuando el dispositivo no esta conectado
            //puesto que para dicha comprobacion al existe la posibilidad de que aun no se haya realizado la conexion y no exista el soket 
            try
            {
                //si existe la conexxion manda el comando especificado
                if (btSocket.IsConnected)
                {
                    //para uso de este programa el comando de retroceso es la letra "S"
                    WriteData(new Java.Lang.String("S"));
                }

            }
            catch (Exception)
            {
            }
        }
        //Vuelta a la derecha
        private void Right_Click(object sender, EventArgs e)
        {
            //el comando try es utilizado cuando el dispositivo no esta conectado
            //puesto que para dicha comprobacion al existe la posibilidad de que aun no se haya realizado la conexion y no exista el soket 
            try
            {
                //si existe la conexxion manda el comando especificado
                if (btSocket.IsConnected)
                {
                    //para uso de este programa el comando de giro a la derecha es la letra "D"
                    WriteData(new Java.Lang.String("D"));
                }
            }
            catch (Exception)
            {
            }
        }
        //Vuelta a la izquierda
        private void Left_Click(object sender, EventArgs e)
        {
            //el comando try es utilizado cuando el dispositivo no esta conectado
            //puesto que para dicha comprobacion al existe la posibilidad de que aun no se haya realizado la conexion y no exista el soket 
            try
            {
                //si existe la conexxion manda el comando especificado
                if (btSocket.IsConnected)
                {
                    //para uso de este programa el comando de giro a la izquierda es la letra "A"
                    WriteData(new Java.Lang.String("A"));
                }
            }
            catch (Exception)
            {
            }
        }
        //detener carro
        private void Stop_Click(object sender, EventArgs e)
        {
            //el comando try es utilizado cuando el dispositivo no esta conectado
            //puesto que para dicha comprobacion al existe la posibilidad de que aun no se haya realizado la conexion y no exista el soket 
            try
            {
                //si existe la conexxion manda el comando especificado
                if (btSocket.IsConnected)
                {
                    //para uso de este programa el comando de paro es la letra "X"
                    WriteData(new Java.Lang.String("X"));
                }
            }
            catch (Exception)
            {
            }
        }
        //metodo para revision de conexion bluetooth
        private void CheckBt()
        {
            //asignamos el sensor bluetooth con el que vamos a trabajar
            //seleccipnando el sensor por defecto del dispositivo
            BlueAdapter = BluetoothAdapter.DefaultAdapter;
            try
            {
                //revisamos si existe
                if (BlueAdapter == null)
                {
                    //en caso de no existir mandamos un mensaje en pantalla
                    Errores.Text = "Bluetooth No Existe o esta Ocupado";
                    //y generamos su exepcion
                    throw new NullReferenceException();
                }
                //Verificamos que este habilitado
                if (BlueAdapter.IsEnabled)
                {

                    Toast.MakeText(this, "Bluetooth activado",
                        ToastLength.Short).Show();
                }
                else
                {
                    Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                    //startActivityForResult(enableBtIntent, );
                }


            }
            catch (NullReferenceException e)
            {
                Toast.MakeText(this, e.Message,
                    ToastLength.Short).Show();
            }
            catch (Exception e)
            {
                Toast.MakeText(this, e.Message,
                    ToastLength.Short).Show();

            }
        }


        public bool Connect()
        {
            //Iniciamos la conexion con el arduino

            Errores.Text = "Conexion en curso...S";

            //Indicamos al adaptador que ya no sea visible

            try
            {

                BluetoothDevice device = BlueAdapter.GetRemoteDevice(address);
                BlueAdapter.CancelDiscovery();
                //Inicamos el socket de comunicacion con el arduino
                Java.Lang.Class[] clase = new Java.Lang.Class[] { Java.Lang.Integer.Type };
                btSocket = (BluetoothSocket)device.Class.GetMethod("createRfcommSocket", clase).Invoke(device, 1);
                //Conectamos el socket
                btSocket.Connect();
                Errores.Text += "\nConexion Correcta";
            }
            catch (Java.Lang.IllegalArgumentException)
            {
                Errores.Text = "dispositivo no encontrado intente conectar nuevamente el dipositivo";
                return false;
            }
            catch (IOException e)
            {

            }
            catch (Exception e)
            {
                //en caso de generarnos error cerramos el socket
                try
                {
                    btSocket.Close();
                }
                catch (Exception)
                {
                    Errores.Text += "Imposible Conectar";
                }
                //System.Console.WriteLine("Socket Creado");
                return false;
            }
            return true;
        }

        private void WriteData(Java.Lang.String Data)
        {
            //Extraemos el stream de salida
            try
            {
                outStream = btSocket.OutputStream;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al enviar" + e.Message);
            }
            //convertimos los datos de envio en bytes
            byte[] msgBuffer = Data.GetBytes();

            try
            {
                //Escribimos en el buffer el arreglo que acabamos de generar
                outStream.Write(msgBuffer, 0, msgBuffer.Length);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Error al enviar" + e.Message);
            }
            outStream.Close();
        }
    }
}