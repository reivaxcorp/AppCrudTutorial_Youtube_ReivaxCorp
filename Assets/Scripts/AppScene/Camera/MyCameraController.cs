/*********************************************************************************
 * Nombre del Archivo:     MyCameraController.cs
 * Descripci�n:            Movemos la camara por medio de touch and drag
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

public class MyCameraController : MonoBehaviour
{
    private float panSpeed = 20f;
    private float zoomSpeed = 5f;

    private void Start()
    {
        if(Application.isMobilePlatform)
        {
            this.panSpeed = 0.5f;
            this.zoomSpeed = 0.1f;
        } else
        {
            this.panSpeed = 50f;
            this.zoomSpeed = 17f;
        }
    }

    private void Update()
    {
        // Movimiento de la c�mara mediante deslizamiento del dedo o del mouse
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            Vector3 move = new Vector3(-touchDeltaPosition.x, 0, -touchDeltaPosition.y) * panSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
        else if (Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Vector3 move = new Vector3(-mouseX, 0, -mouseY) * panSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }

        // Acercamiento de la c�mara con dos dedos o con la rueda del mouse
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Camera.main.fieldOfView += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;

            // Limitar el campo de visi�n para evitar acercamiento excesivo
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 10f, 90f);
        }
        else
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Camera.main.fieldOfView -= scroll * zoomSpeed * 100f * Time.deltaTime;

            // Limitar el campo de visi�n para evitar acercamiento excesivo
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 10f, 90f);
        }
    }
}
