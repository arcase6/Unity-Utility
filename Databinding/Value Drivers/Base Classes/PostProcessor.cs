using System;
using System.Collections.Generic;
using UnityEngine;


public interface PostProcessor<U>{
    U Proccess(U unproccessedValue);

}


