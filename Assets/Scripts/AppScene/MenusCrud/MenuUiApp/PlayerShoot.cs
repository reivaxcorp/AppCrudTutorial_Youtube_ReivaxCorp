/*********************************************************************************
* Nombre del Archivo:     PlayerShoot.cs 
           realizado un touch o un click en el editor. Si esta bala colisiona con algun �tem
 *                         en la escena, se abrira su men * Descripci�n:            Clase encargada de inicializar, una bala que se dirigira hacia el usuario haya
 *              u para editar.
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

using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] GameObject gunPlayer;
    [SerializeField] GameObject bulletPrefb; // Prefab del objeto que quieres lanzar
    [SerializeField] private float powerBullet = 2000f;

    void Update()
    {
        BulletRayCast();
    }

    private void BulletRayCast()
    {
        // Verificar si se ha tocado la pantalla en dispositivos t�ctiles o se ha hecho clic con el mouse
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            // Verificar si el puntero est� sobre un elemento de la interfaz de usuario
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Obtener la posici�n del toque en la pantalla o del clic del mouse
                Vector3 touchPosition;

                // Verificar si se est� ejecutando en un dispositivo t�ctil
                if (Input.touchCount > 0)
                {
                    // Obtener la posici�n del toque en la pantalla
                    touchPosition = Input.GetTouch(0).position;
                }
                // Verificar si se est� ejecutando en el editor de Unity y se ha hecho clic con el mouse
                else if (Input.GetMouseButtonDown(0))
                {
                    // Obtener la posici�n del clic del mouse
                    touchPosition = Input.mousePosition;
                }
                else
                {
                    // En caso de que no haya ni toques ni clics, se asigna un valor por defecto
                    touchPosition = Vector3.zero;
                }

                // Convertir la posici�n del toque de pantalla a un rayo en el mundo
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);

                // Dibujar el rayo en la escena
                Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.green, 5f);

                // Lanzar el objeto en la direcci�n del rayo
                LaunchObject(ray);
            }
        }
    }

    void LaunchObject(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("Item box hit.. " + hit.collider);

            // Obtener la posici�n donde el rayo colision� con la superficie
            Vector3 targetPosition = hit.point;

            // Instanciar el objeto a lanzar en la posici�n del jugador
            GameObject bulet = GetBulletInitialized();

            // Calcular la direcci�n hacia la posici�n del objetivo
            Vector3 direction = (targetPosition - bulet.transform.position).normalized;

            // Aplicar fuerza al objeto lanzado en la direcci�n del rayo
            bulet.GetComponent<Rigidbody>().AddForce(direction * powerBullet);
        }
        else
        {
            // Instanciar el objeto a lanzar en la posici�n del jugador
            GameObject bullet = GetBulletInitialized();
            // Iniciar la coroutine para mover el objeto hacia la posici�n del objetivo

            // Calcular la direcci�n hacia la posici�n del objetivo
            Vector3 direction = ray.direction.normalized;

            // Aplicar fuerza al objeto lanzado en la direcci�n del rayo
            bullet.GetComponent<Rigidbody>().AddForce(direction * powerBullet);
        }
    }

    private GameObject GetBulletInitialized()
    {
        // Instanciar el objeto a lanzar en la posici�n del jugador
        GameObject bullet = Instantiate(bulletPrefb, gunPlayer.transform.position, Quaternion.identity);
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();    
        if(bulletScript != null)
        {
            MenuManagerApp menuManagerApp = gameObject.GetComponent<MenuManagerApp>();
            if(menuManagerApp != null)
            {
                bulletScript.SetMenuManager(menuManagerApp);
            } else
            {
                Debug.LogWarning("MenuMnagerApp.cs, no existe en el inspector en UiApp");
            }
        } else
        {
            Debug.LogWarning("bulletScript.cs, no existe en el Bullet prefab");
        }
        return bullet;
    }
}