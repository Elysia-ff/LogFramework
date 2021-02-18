using UnityEngine;
using UnityEngine.UI;

namespace LogFramework.Sample
{
    public class SampleScript : MonoBehaviour
    {
        public enum MyEnum
        {
            My_1,
            My_2,
            My_3,
            My_4
        }

        public enum YourEnum
        {
            Your_1,
            Your_2
        }

        public void OnButton_1()
        {
            LogSystem.Instance.Send(new LogButtonClick());
        }

        public void OnButton_2()
        {
            LogSystem.Instance.Send(new LogButtonClick2());
        }

        public void OnDropdown()
        {
            MyEnum myEnum = (MyEnum)GameObject.Find("Dropdown").GetComponent<Dropdown>().value;
            if (LogDropDown.IsValid(myEnum))
            {
                LogSystem.Instance.Send(new LogDropDown(myEnum));
            }
            else
            {
                Debug.Log("I don't want to send this enum log! so this won't be shown on LogFinder either");
            }
        }

        public void OnDualDropdown()
        {
            MyEnum myEnum = (MyEnum)GameObject.Find("Dropdown (1)").GetComponent<Dropdown>().value;
            YourEnum yourEnum = (YourEnum)GameObject.Find("Dropdown (2)").GetComponent<Dropdown>().value;

            LogSystem.Instance.Send(new LogDualDropDown(myEnum, yourEnum));
        }

        public void OnToggle()
        {
            bool value = GameObject.Find("Toggle").GetComponent<Toggle>().isOn;
            LogSystem.Instance.Send(new LogToggle(value));
        }

        public void OnTable()
        {
            int tableIdx = GameObject.Find("Dropdown (3)").GetComponent<Dropdown>().value + 1;

            if (LogTable.IsValid(tableIdx))
            {
                LogSystem.Instance.Send(new LogTable(tableIdx));
            }
            else
            {
                Debug.Log("The log_name field of the table is empty! so don't send it");
            }
        }

        public void OnSlider()
        {
            int value = (int)GameObject.Find("Slider").GetComponent<Slider>().value;
            LogSystem.Instance.Send(new LogSlider(value));
        }
    }
}
