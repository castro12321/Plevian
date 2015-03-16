using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Buildings;
using Plevian.TechnologY;
using Plevian.Units;

namespace Plevian.Villages
{
    public class VillageQueues
    {
        /// <summary>
        /// Base QueueItem class for other queue items (BuildingQueueItem, RecruitQueueItem, TechnologyQueueItem)
        /// </summary>
        public class QueueItem
        {
            public readonly String Name;
            public readonly String Extra;
            public readonly GameTime Start;
            public readonly GameTime End;

            public QueueItem(String Name, String Extra, GameTime Start, GameTime End)
            {
                this.Name = Name;
                this.Extra = Extra;
                this.Start = Start;
                this.End = End;
            }
        }

        private Village owner;

        public VillageQueues(Village owner)
        {
            this.owner = owner;
        }

        /// <summary>Called when when new item has been added to the queue</summary>
        /// <param name="village">May be useful for WPF</param>
        /// <param name="item">Item added to the queue</param>
        public delegate void QueueItemAdded(Village village, QueueItem item);
        /// <summary>Called when the time passed for the queue item</summary>
        public delegate void QueueItemFinished(Village village, QueueItem item);
        /// <summary>Called when an item is being removed before the time pass. For example, user canceled the task</summary>
        public delegate void QueueItemRemoved(Village village, QueueItem item);

        // Events that village and WPF may listen to.
        public event QueueItemAdded queueItemAdded;
        /// <summary>
        /// Event for the village to listen. (finishBuilding, finishRecruiting and finishResearching merges into FinishQueueItem(Village village, QueueItem item))
        /// When an item is finished, village checks which (BuildingQueueItem, RecruitQueueItem, TechnologyQueueItem)
        /// and then takes an appropriate action
        /// For example:
        /// BuildingQueueItem buildingItem = queueItem as BuildingQueueItem;
        /// if(buildingItem != null)
        /// { ... }
        /// then check for RecruitQueueItem and TechnologyQueueItem like above
        /// </summary>
        public event QueueItemFinished queueItemFinished;
        public event QueueItemRemoved queueItemRemoved;

        public ObservableCollection<QueueItem> queue = new ObservableCollection<QueueItem>();
        public ObservableCollection<BuildingQueueItem> buildingQueue = new ObservableCollection<BuildingQueueItem>();
        public ObservableCollection<RecruitQueueItem> recruitQueue = new ObservableCollection<RecruitQueueItem>();
        public ObservableCollection<ResearchQueueItem> researchQueue = new ObservableCollection<ResearchQueueItem>();

        public void Add(QueueItem item)
        {
            queue.Add(item);
            if (item is BuildingQueueItem) buildingQueue.Add(item as BuildingQueueItem);
            else if (item is RecruitQueueItem) recruitQueue.Add(item as RecruitQueueItem);
            else if (item is ResearchQueueItem) researchQueue.Add(item as ResearchQueueItem);

            sort();
            if (queueItemAdded != null)
                queueItemAdded(owner, item);
        }

        public void Remove(QueueItem item)
        {
            queue.Remove(item);
            if (item is BuildingQueueItem) buildingQueue.Remove(item as BuildingQueueItem);
            else if (item is RecruitQueueItem) recruitQueue.Remove(item as RecruitQueueItem);
            else if (item is ResearchQueueItem) researchQueue.Remove(item as ResearchQueueItem);

            sort();
            if (queueItemRemoved != null)
                queueItemRemoved(owner, item);
        }

        /// <summary>
        /// Called by the village every tick
        /// </summary>
        public void CompleteAvailableItems()
        {
            while (queue.Count > 0)
            {
                QueueItem item = queue[0];
                if (GameTime.now < item.End)
                    return;

                queue.RemoveAt(0);
                if (item is BuildingQueueItem) buildingQueue.RemoveAt(0);
                else if (item is RecruitQueueItem) recruitQueue.RemoveAt(0);
                else if (item is ResearchQueueItem) researchQueue.RemoveAt(0);

                if (queueItemFinished != null)
                    queueItemFinished(owner, item);
            }
        }

        public bool isResearching(Technology technology)
        {
            foreach (ResearchQueueItem item in researchQueue)
                if (item.researched == technology)
                    return true;
            return false;
        }

        private void sort()
        {
            var ordered = queue.OrderBy(item => item.End.time).ToList();
            queue.Clear();
            foreach (QueueItem item in ordered)
                queue.Add(item);
            // Don't sort building/recruit/research queues on its own
            // They are sorted by default because they are finished in the same order they were placed
        }
    }
}
