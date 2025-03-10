﻿using System.Windows.Forms;

namespace gMediaTools.Extensions
{
    public static class ListBoxExtension
    {
        public static void MoveSelectedItemUp(this ListBox listBox)
        {
            MoveSelectedItemInner(listBox, -1);
        }

        public static void MoveSelectedItemDown(this ListBox listBox)
        {
            MoveSelectedItemInner(listBox, 1);
        }

        static void MoveSelectedItemInner(ListBox listBox, int direction)
        {
            // Checking selected item
            if (listBox.SelectedItem == null || listBox.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = listBox.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= listBox.Items.Count)
                return; // Index out of range - nothing to do

            object selected = listBox.SelectedItem;

            // Save checked state if it is applicable
            var checkedListBox = listBox as CheckedListBox;
            var checkState = CheckState.Unchecked;
            if (checkedListBox != null)
                checkState = checkedListBox.GetItemCheckState(checkedListBox.SelectedIndex);

            // Removing removable element
            listBox.Items.Remove(selected);
            // Insert it in new position
            listBox.Items.Insert(newIndex, selected);
            // Restore selection
            listBox.SetSelected(newIndex, true);

            // Restore checked state if it is applicable
            if (checkedListBox != null)
                checkedListBox.SetItemCheckState(newIndex, checkState);
        }
    }
}
