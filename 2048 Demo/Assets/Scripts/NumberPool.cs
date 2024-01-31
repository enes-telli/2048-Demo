using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPool : Singleton<NumberPool>
{
    [SerializeField] private List<GameObject> _pooledNumbers = new List<GameObject>();
    
    public GameObject GetPooledObject(int numberToGet, Transform targetNode, bool wantActive)
    {
        for (int i = 0; i < _pooledNumbers.Count; i++)
        {
            GameObject currentNumber = _pooledNumbers[i];

            if (currentNumber.name.Equals(numberToGet.ToString()) && !currentNumber.GetComponent<Number>().isUsing)
            {
                currentNumber.GetComponent<Number>().isUsing = true;
                currentNumber.SetActive(wantActive);
                currentNumber.transform.SetParent(targetNode);
                currentNumber.transform.localScale = Vector3.one;
                currentNumber.transform.localPosition = Vector3.zero;
                return currentNumber;
            }
        }

        GameObject newNumber = Instantiate(Resources.Load("Nodes/" + numberToGet.ToString()), targetNode) as GameObject;
        newNumber.name = numberToGet.ToString();
        _pooledNumbers.Add(newNumber);
        return newNumber;
    }

    public void SetPooledObject(Transform numberToPool)
    {
        numberToPool.GetComponent<Number>().isUsing = false;
        numberToPool.gameObject.SetActive(false);
        numberToPool.SetParent(transform);
    }
}
