using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawningWaveElement : WaveElement {

	[Tooltip("The object to get spawned when this entry gets triggered")]
	public GameObject prefab;
	[Tooltip("The tag that the object should get assigned")]
	public string assignedTag = "Untagged";

	public virtual void BeforeSpawn(Action spawnTask) => spawnTask.Invoke();

	public void Spawn(Vector3 position, Transform parent, Action<List<GameObject>> callback, int amount = 1) {
		var list = new List<GameObject>();

		Action SpawnInternal(bool last) => () => {
			var obj = Instantiate(prefab, position, prefab.gameObject.transform.rotation, parent);
			var spawnable = obj.GetComponent<WaveSpawnable>();
			spawnable.objectTag = assignedTag;
			spawnable.OnSpawned(position, this);
			AfterSpawn(obj);
			list.Add(obj);
			if (last) callback.Invoke(list);
		};

		for (var i = 0; i < amount; i++) {
			BeforeSpawn(SpawnInternal(i + 1 >= amount));
		}
	}

	public virtual void AfterSpawn(GameObject obj) { }

}