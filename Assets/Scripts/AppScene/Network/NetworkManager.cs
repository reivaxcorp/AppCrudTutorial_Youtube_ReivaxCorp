/*********************************************************************************
 * Nombre del Archivo:     NetworkManager.cs 
 * Descripción:            Verificamos que tangamos conexion a internet para poder añadir nuevos
 *                         ítems en la aplicación, en caso de que no hay internet, se deshabilitaran los 
 *                         botónes correspondientes a añadir ítem en el script ManageItems.cs.
 *                         
 * Autor:                  Javier
 * Organización:           ReivaxCorp.
 *
 * Derechos de Autor © [2024] ReivaxCorp
 
 * Se otorga permiso, sin cargo, a cualquier persona para obtener una copia de este software y de los archivos de
 * documentación asociados (el “Software”), para tratar con el Software sin restricciones, incluyendo, pero no
 * limitado a, los derechos para usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias 
 * del Software, y para permitir a las personas a quienes pertenezca el Software hacer lo mismo, sujeto a las
 * siguientes condiciones:
 
 * El aviso de derechos de autor anterior y este aviso de permiso se incluirán en todas las copias o partes
 * sustanciales del Software realizadas por el desarrollador, específicamente en las carpetas “Assets/Scripts”.
 
 * Las partes de plugins y recursos provenientes de la Asset Store de Unity 3D están sujetas a los derechos de autor 
 * de los respectivos desarrolladores o artistas, así como a las políticas de Unity 3D.
 
 * EL SOFTWARE SE PROPORCIONA “TAL CUAL”, SIN GARANTÍA DE NINGÚN TIPO, EXPRESA O IMPLÍCITA, 
 * INCLUYENDO, PERO NO LIMITADO A, LAS GARANTÍAS DE COMERCIABILIDAD, IDONEIDAD PARA UN 
 * PROPÓSITO PARTICULAR Y NO INFRACCIÓN. EN NINGÚN CASO LOS AUTORES O TITULARES DE DERECHOS DE 
 * AUTOR SERÁN RESPONSABLES DE CUALQUIER RECLAMACIÓN, DAÑO U OTRA RESPONSABILIDAD, YA SEA EN 
 * UNA ACCIÓN DE CONTRATO, AGRAVIO U OTRO MOTIVO, DERIVADA DE, FUERA DE O EN CONEXIÓN CON EL
 * SOFTWARE O EL USO U OTROS TRATOS EN EL SOFTWARE.
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

        // Llamar al método para manejar la conectividad
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

                // Llamar al método para manejar la conectividad
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
                // Debug.Log("No hay conexión a Internet.");
                handleInternetAvariableResult?.Invoke(false);
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                // Debug.Log("Conexión a Internet disponible.");
                handleInternetAvariableResult?.Invoke(true);
                break;
        }
    }
}
