using System;
using System.Windows.Forms;
using System.Drawing;
using Sony.Vegas;




public class EntryPoint

{

    public void FromVegas(Vegas vegas)
    {

        try
        {
            Track track = vegas.Project.Tracks[0];

            float rescale = 0.9f;
            double new_duration;

            foreach (TrackEvent curr_event in track.Events)
            {
                new_duration = curr_event.Length.ToMilliseconds() * rescale;
                curr_event.AdjustStartLength(curr_event.Start, Timecode.FromMilliseconds(new_duration), false);
            }




            Timecode curr_event_length;
            Timecode curr_event_start;
            Timecode next_event_start = Timecode.FromSeconds(0);

            foreach (TrackEvent curr_event in track.Events)
            {
                curr_event_length = curr_event.End;
                curr_event_start = curr_event.Start;

                curr_event.AdjustStartLength(next_event_start, curr_event.Length, false);
                next_event_start = curr_event.End;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }


    }
}