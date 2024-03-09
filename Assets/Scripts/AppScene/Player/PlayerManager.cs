/*********************************************************************************
 * Nombre del Archivo:     PlayerManager.cs 
 * Descripci�n:            Clase que nos ayudara a resetear la posici�n de nuestro jugador, 
 *                         interactura con el plugin de Cinemchine, hay que desactivar algunos
 *                         componentes, para poder trasladar a nuestro personaje.
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

using StarterAssets;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Vector3 _initialPosition;
    private CharacterController characterController;
    private ThirdPersonController thirdPersonController;

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        InitRef();
    }

    public void ResetOriginalPosition()
    {
        if (characterController != null)
        {
            transform.position = _initialPosition;
            DisableController(false);
        }
    }

    /// <summary>
    /// Para trasladar nuestro personaje es necesario deshabilitar los scrips de
    /// Started Assets
    /// </summary>
    /// <param name="isDisable"></param>
    public void DisableController(bool isDisable)
    {
        if (characterController != null && thirdPersonController != null)
        {
            characterController.enabled = !isDisable;
            thirdPersonController.enabled = !isDisable;
        }
    }

    private void InitRef()
    {
        characterController = GetComponent<CharacterController>();
        if(characterController == null ) { Debug.LogWarning("Deberia haber un CharacterController"); }
        thirdPersonController = GetComponent<ThirdPersonController>();
        if (thirdPersonController == null) { Debug.LogWarning("Deberia haber un ThirdPersonController"); }
    }
}
