/*********************************************************************************
 * Nombre del Archivo:     FirebaseSDK.cs
 * Descripci�n:            Inicializamos las dependencias de firebase, y escuchamos los cambios de 
 *                         inicio de sesi�n de usuario. 
 *                         
 * Autor:                  Javier
 * Organizaci�n:           ReivaxCorp.
 *
 * Derechos de Autor � [2024] ReivaxCorp
 
 * Se otorga permiso, sin cargo, a cualquier persona para obtener una copia de este software y de los archivos de
 * documentaci�n asociados (el �Software�), para tratar con el Software sin restricciones, incluyendo, pero no
 * limitado a, los derechos para usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias 
 * del Software, y para permitir a las personas a quienes pertenezca el Software hacer lo mismo, sujeto a las
 * siguientes condiciones:
 
 * El aviso de derechos de autor anterior y este aviso de permiso se incluir�n en todas las copias o partes
 * sustanciales del Software realizadas por el desarrollador, espec�ficamente en las carpetas �Assets/Scripts�.
 
 * Las partes de plugins y recursos provenientes de la Asset Store de Unity 3D est�n sujetas a los derechos de autor 
 * de los respectivos desarrolladores o artistas, as� como a las pol�ticas de Unity 3D.
 
 * EL SOFTWARE SE PROPORCIONA �TAL CUAL�, SIN GARANT�A DE NING�N TIPO, EXPRESA O IMPL�CITA, 
 * INCLUYENDO, PERO NO LIMITADO A, LAS GARANT�AS DE COMERCIABILIDAD, IDONEIDAD PARA UN 
 * PROP�SITO PARTICULAR Y NO INFRACCI�N. EN NING�N CASO LOS AUTORES O TITULARES DE DERECHOS DE 
 * AUTOR SER�N RESPONSABLES DE CUALQUIER RECLAMACI�N, DA�O U OTRA RESPONSABILIDAD, YA SEA EN 
 * UNA ACCI�N DE CONTRATO, AGRAVIO U OTRO MOTIVO, DERIVADA DE, FUERA DE O EN CONEXI�N CON EL
 * SOFTWARE O EL USO U OTROS TRATOS EN EL SOFTWARE.
 *********************************************************************************/

using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseSDK
{
    public FirebaseApp app
    {
        private set { _app = value; }
        get { return _app; }
    }
    public FirebaseDatabase defaultInstance
    {
        private set { _defaultInstance = value; }
        get { return _defaultInstance; }
    }
    public FirebaseStorage firebaseStorage
    {
        private set { _firebaseStorage = value; }
        get { return _firebaseStorage; }
    }
    public FirebaseAuth auth
    {
        private set { _auth = value; }
        get { return _auth; }
    }
    public FirebaseUser user
    {
        private set { _user = value; }
        get { return _user; }
    }
    public bool isFirebaseReady
    {
        private set { _isFirebaseReady = value; }
        get { return _isFirebaseReady; }
    }

    private static FirebaseSDK instance;
    private FirebaseApp _app;
    private FirebaseDatabase _defaultInstance;
    private FirebaseStorage _firebaseStorage;
    private FirebaseAuth _auth;
    private FirebaseUser _user;
    private bool _isFirebaseReady;


    /// <summary>
    /// Initialize firebase dependencies. 
    /// </summary>
    /// <returns></returns>
    public async Task<bool> InitFirebaseDependenciesAsync()
    {

        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.

                this.app = Firebase.FirebaseApp.DefaultInstance; // PRODUCTION MODE

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                this.defaultInstance = FirebaseDatabase.DefaultInstance;

                this.firebaseStorage = FirebaseStorage.DefaultInstance;

                this.auth = FirebaseAuth.DefaultInstance;
                this.auth.StateChanged += AuthStateChanged;

                isFirebaseReady = true;
                AuthStateChanged(this, null);
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                isFirebaseReady = false;
                throw new Exception(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
       
        return isFirebaseReady;
    }

    /// <summary>
    ///   La primera vez que entre el usuario, sera null.
    ///   Una vez que inicie sesi�n, tendremos un usuario, y podremos setear 
    ///   las propiedades de neustras base de datos y la app, para guardar lo datos.
    /// </summary>
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }

        InitUidUserToApp(); // podemos inicializar ahora los datos principales.
    }

    public static FirebaseSDK GetInstance()
    {
        if (instance == null)
        {
            instance = new FirebaseSDK();
        }
        return instance;
    }

    public void LogOut()
    {
        if (auth != null)
        {
            auth.SignOut();
        }
    }

    /// <summary>
    /// Inicializamos el uid del usuario en la base de datos remota y local, asi como tambien
    /// en el Texture manager, ya que necesitamos para acceder a los datos de dicho usuario logeado
    /// </summary>
    private void InitUidUserToApp()
    {
        if (user != null && MyApplication.repository != null)
        {
            MyApplication.repository.GetRemoteDb().SetUserUid(user.UserId);
            MyApplication.repository.GetLocalDb().SetUserUidFolder(user.UserId);
        } else
        {
            MyApplication.repository.GetRemoteDb().SetUserUid(null);
            MyApplication.repository.GetLocalDb().SetUserUidFolder(null);
            Debug.Log("Usuario inexistente por ahora..");
        }
    }

    private void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
}
