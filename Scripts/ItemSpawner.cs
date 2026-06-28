using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrefabs;
    [SerializeField] ItemCollisionHandler itemCollisionHandler;
    [SerializeField] int itemCount = 20;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < itemCount; i++)
        {
            GameObject itemInstance = Instantiate(itemPrefabs[Random.Range(0,itemPrefabs.Length)], itemCollisionHandler.transform.position, Random.rotation);
            itemInstance.transform.SetParent(itemCollisionHandler.transform);

            Vector3 finalPos = itemCollisionHandler.transform.position + Random.insideUnitSphere * itemCollisionHandler.collisionRadius
                * itemCollisionHandler.spreadMultiplier;

            itemInstance.transform.DOMove(finalPos, .2f);

            Vector3 finalScale = itemInstance.transform.localScale;
            itemInstance.transform.localScale = Vector3.zero;
            itemInstance.transform.DOScale(finalScale, .5f);

            yield return new WaitForSeconds(.001f);
        }
    }
}
