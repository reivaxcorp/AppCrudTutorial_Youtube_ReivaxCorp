/*********************************************************************************
 * Nombre del Archivo:     MyApplication.cs 
 * Descripci�n:            Inicializamos nuestro repositorio, los servicios de firebase
 *                         
 * Autor:                  Javier
 * Organizaci�n:           ReivaxCorp.
 *
 * Derechos de Autor (c) [2024] ReivaxCorp
 * 
 * Permiso es otorgado, sin cargo, para que cualquier persona obtenga una copia
 * de este software y de los archivos de documentaci�n asociados (el "Software"),
 * para tratar en el Software sin restricci�n, incluyendo sin limitaci�n los
 * derechos para usar, copiar, modificar, fusionar, publicar, distribuir,
 * sublicenciar, y/o vender copias del Software, y para permitir a las personas a
 * quienes pertenezca el Software, sujeto a las siguientes condiciones:
 *
 * El aviso de derechos de autor anterior y este aviso de permiso se incluir�n en
 * todas las copias o partes sustanciales del Software.
 *
 * EL SOFTWARE SE PROPORCIONA "TAL CUAL", SIN GARANT�A DE NING�N TIPO, EXPRESA O
 * IMPL�CITA, INCLUYENDO PERO NO LIMITADO A LAS GARANT�AS DE COMERCIABILIDAD,
 * IDONEIDAD PARA UN PROP�SITO PARTICULAR Y NO INFRACCI�N. EN NING�N CASO LOS
 * AUTORES O TITULARES DE DERECHOS DE AUTOR SER�N RESPONSABLES DE CUALQUIER
 * RECLAMACI�N, DA�O O OTRA RESPONSABILIDAD, YA SEA EN UNA ACCI�N DE CONTRATO, AGRAVIO
 * O DE OTRO MODO, DERIVADAS DE, FUERA DE O EN CONEXI�N CON EL SOFTWARE O EL USO U OTROS
 * TRATOS EN EL SOFTWARE.
 *********************************************************************************/

using System;
using System.Threading.Tasks;
using UnityEngine;

public class MyApplication : MonoBehaviour
{
    public static MyRepository repository
    {
        private set { _repository = value; }
        get { return _repository; }
    }
    private static MyRepository _repository;


    private async void Start()
    {
       if(repository == null)
        {
            await CreateRepositoryAsync();
        }
    }

    private async Task<MyRepository> CreateRepositoryAsync()
    {

        RemoteDb remoteDb = new RemoteDb();
        LocalDb localDb = new LocalDb();

        repository = new MyRepository(localDb, remoteDb);

        // FIRST wait to initialize Sdk Firebase
        bool isInitialize = await InicializeFirebase();

        if (isInitialize)
        {
            return repository;
        }
        else
        {
            Debug.LogWarning("Error al crear el respositorio");
            return null;
        }
    }

    // we wait firebase start
    private async Task<bool> InicializeFirebase()
    {
        FirebaseSDK firebaseSdk = FirebaseSDK.GetInstance();

        try
        {
            bool firebaseInitialized = await firebaseSdk.InitFirebaseDependenciesAsync();

            if (firebaseInitialized)
            {
                Debug.Log($"Firebase running");
                return firebaseInitialized;
            }
            else
            {
                // Handle the exception where Firebase initialization failed.
                Debug.Log($"Firebase initialization it's false");
                return false;
            }
        }
        catch (Exception ex)
        {
            // Handle the exception where Firebase initialization failed.
            Debug.Log($"Firebase initialization error: {ex.Message}");
            return false;
        }
    }

}
