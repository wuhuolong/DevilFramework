﻿using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devil.AI
{
    public abstract class BTTaskBase
    {
        public int Id { get; private set; }

        public BTTaskBase(int id)
        {
            Id = id;
        }

        public abstract void OnInitData(BehaviourTreeRunner btree, string jsonData);

        public abstract void OnAbort(BehaviourTreeRunner btree);

        public abstract EBTTaskState OnTaskStart(BehaviourTreeRunner btree);

        public abstract EBTTaskState OnTaskTick(BehaviourTreeRunner btree, float deltaTime);

    }
}