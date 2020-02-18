using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace egrath.tools.algo
{
    public class RouletteWheelSelection<T>
    {
        private List<RouletteItem> m_Items;
        private System.Random m_RandomGenerator = new System.Random();

        public RouletteWheelSelection()
        {
            m_Items = new List<RouletteItem>();
        }

        /// <summary>
        /// Adds a new Item to the Wheel
        /// </summary>
        /// <param name="item"></param>
        /// <param name="score"></param>
        public void Add(T item, double score)
        {
            m_Items.Add(new RouletteItem(item, m_Items.Count == 0 ? 0 : m_Items[m_Items.Count - 1].TotalScore + score, score));
        }

        /// <summary>
        /// Returns a Item according to the Roulette Wheel Algorithm
        /// </summary>
        /// <returns>Selected Item</returns>
        public T GetItem(bool remove)
        {
            if (m_Items.Count == 0)
                return default(T);

            int select = m_RandomGenerator.Next( (int)m_Items[m_Items.Count - 1].TotalScore );
            for (int index = 0; index < m_Items.Count; index++)
            {
                double lowerLimit = 0;
                if (index > 0)
                    lowerLimit = m_Items[index - 1].TotalScore;

                double upperLimit = m_Items[index].TotalScore;

                if (select >= lowerLimit && select <= upperLimit)
                {
                    T item = m_Items[index].Item;
                    if (remove)
                    {
                        RemoveItem(index);
                    }

                    return item;
                }
            }

            throw (new System.Exception("RouletteWheelSelection internal error!"));
        }

        /// <summary>
        /// Number of Items contained within the Wheel
        /// </summary>
        public int NumItems
        {
            get { return m_Items.Count; }
        }

        /// <summary>
        /// Internal usage: Removes the Item at the specified index from the Wheel
        /// </summary>
        /// <param name="index"></param>
        private void RemoveItem(int index)
        {
            double baseScore = m_Items[index].TotalScore - m_Items[index].ItemScore;
            m_Items.RemoveAt(index);
            for (int i = index; i < m_Items.Count; i++)
            {
                m_Items[index].TotalScore = baseScore + m_Items[index].ItemScore;
                baseScore = m_Items[index].TotalScore;
            }
        }

        #region Roulette Item
        private class RouletteItem
        {
            private T m_Item;
            private double m_ItemScore;
            private double m_TotalScore;

            public RouletteItem(T item, double score, double itemScore)
            {
                Item = item;
                TotalScore = score;
                ItemScore = itemScore;
            }

            public T Item
            {
                get
                {
                    return m_Item;
                }

                set
                {
                    m_Item = value;
                }
            }

            public double ItemScore
            {
                get
                {
                    return m_ItemScore;
                }

                set
                {
                    m_ItemScore = value;
                }
            }

            public double TotalScore
            {
                get
                {
                    return m_TotalScore;
                }

                set
                {
                    m_TotalScore = value;
                }
            }
        }
        #endregion
    }


}
