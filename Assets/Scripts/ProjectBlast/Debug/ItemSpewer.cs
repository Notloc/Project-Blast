using ProjectBlast.Game;
using ProjectBlast.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBlast.Debug
{
    public class ItemSpewer : MonoBehaviour, IActivate
    {
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] List<ItemBase> items = new List<ItemBase>();

        [SerializeField] float delay = 0f;
        [SerializeField] float launchSpeed = 7f;
        [SerializeField] float speedVariance = 0.5f;

        private float lastItemTime = -100f;
        private int itemsSpawned = 0;



        private bool isOn = false;

        public void Activate()
        {
            isOn = !isOn;
        }

        private void FixedUpdate()
        {
            if (!isOn)
                return;
            if (Time.time > lastItemTime)
            {
                SpawnItem();
                lastItemTime = Time.time + delay;
                itemsSpawned++;
            }
        }

        private void SpawnItem()
        {
            ItemBase randomBase = items[Random.Range(0, items.Count)];
            Item newItem = ItemFactory.Instance.CreateItem(randomBase);
            ItemEntity entity = ItemEntityFactory.Instance.CreateItemEntity(newItem, spawnPoint.position);

            Vector3 randomRotation = new Vector3(
                Random.Range(-15f, 15f),
                Random.Range(-5f, 5f),
                Random.Range(-15f, 15f)
            );

            float speed = launchSpeed + Random.Range(-speedVariance, speedVariance);
            entity.Rigidbody.AddForce(Quaternion.Euler(randomRotation) * (spawnPoint.forward * launchSpeed), ForceMode.VelocityChange);
        }
    }
}