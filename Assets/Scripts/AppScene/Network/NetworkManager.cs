/*********************************************************************************
 * Nombre del Archivo:     NetworkManager.cs 
 * Descripci�n:            Verificamos que tangamos conexion a internet para poder a�adir nuevos
 *                         �tems en la aplicaci�n, en caso de que no hay internet, se deshabilitaran los 
 *                         bot�nes correspondientes a a�adir �tem en el script ManageItems.cs.
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

using UnityEngine;
 
public class NetworkManager : MonoBehaviour
{
    private NetworkReachability previousReachability;
    public delegate void OnInternetAvariable(bool isInternetAvariable);
    public event OnInternetAvariable handleInternetAvariableResult;
    private bool startListeningInternet;

    private void Start()
    {
        startListeningInternet = false;
    }

    public void ListeningInternetAvariable()
    {
        startListeningInternet = true;
        // Guardar el estado de la conectividad a Internet al inicio
        previousReachability = Application.internetReachability;

        // Llamar al m�todo para manejar la conectividad
        HandleInternetReachability();
    }

    void Update()
    {
        if (startListeningInternet)
        {
            // Comprobar si ha cambiado el estado de la conectividad a Internet
            if (Application.internetReachability != previousReachability)
            {
                // Actualizar el estado anterior de la conectividad
                previousReachability = Application.internetReachability;

                // Llamar al m�todo para manejar la conectividad
                HandleInternetReachability();
            }
        }
    }

    void HandleInternetReachability()
    {
        // Obtener el estado actual de la conectividad a Internet
        NetworkReachability reachability = Application.internetReachability;

        // Comprobar el estado y actuar en consecuencia
        switch (reachability)
        {
            case NetworkReachability.NotReachable:
                // Debug.Log("No hay conexi�n a Internet.");
                handleInternetAvariableResult?.Invoke(false);
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                // Debug.Log("Conexi�n a Internet disponible.");
                handleInternetAvariableResult?.Invoke(true);
                break;
        }
    }
}
