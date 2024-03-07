/*********************************************************************************
 * Nombre del Archivo:     ItemExtensions.cs
 * Descripci�n:            Convierte el �tem de una base de datos remota en un �tem para la 
 *                         base de datos local, y viceversa.  
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

using System.Collections.Generic;

public static class ItemExtensions
{
    public static ItemLocal ItemRemoteToItemLocal(this ItemRemote itemRemote)
    {
        return new ItemLocal
        {
            // Realiza la conversi�n de los campos seg�n sea necesario
            Id = itemRemote.Id,
            Name = itemRemote.Name,
            ImageName = itemRemote.ImageName,
            CreationDate = itemRemote.CreationDate
        };
    }

    public static List<ItemLocal> ItemsRemoteToItemLocal(this List<ItemRemote> itemsRemote)
    {
        List<ItemLocal> itemsLocal = new List<ItemLocal>();

        foreach (var remoteItem in itemsRemote)
        {
            ItemLocal localItem = new ItemLocal
            {
                // Realiza la conversi�n de los campos seg�n sea necesario
                Id = remoteItem.Id,
                Name = remoteItem.Name,
                ImageName = remoteItem.ImageName,
                CreationDate = remoteItem.CreationDate
            };

            itemsLocal.Add(localItem);
        }

        return itemsLocal;
    }

    public static bool IsSameContent(ItemLocal itemLocal, ItemRemote itemRemote)
    {
        if(itemLocal == null && itemRemote == null) return true;

        return
                itemLocal.Id.Equals(itemRemote.Id) &&
                itemLocal.Name.Equals(itemRemote.Name) &&
                itemLocal.ImageName.Equals(itemRemote.ImageName) && 
                itemLocal.CreationDate == itemRemote.CreationDate;
    }
}
