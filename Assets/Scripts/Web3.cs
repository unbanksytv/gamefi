using System.Collections;
using System.Collections.Generic;
using Thirdweb;
using UnityEngine;

public class Web3 : MonoBehaviour
{
    private ThirdwebSDK sdk;

    // Start is called before the first frame update
    void Start()
    {
        sdk = new ThirdwebSDK("goerli");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
