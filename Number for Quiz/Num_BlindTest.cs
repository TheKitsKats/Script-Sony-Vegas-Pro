using System;
using System.Windows.Forms;
using System.Drawing;
using Sony.Vegas;


class Constants
{
    public static string FONT_FAMILY = "Arial";
    public static int TEXT_SIZE = 70;
    public static Color TEXT_COLOR = Color.Cyan;
}


public class EntryPoint

{

    public void FromVegas(Vegas vegas)
    {

        try
        {
            int num = 1;  //number of the current song track
            Timecode curr_song_lenght;    //the lenght of the current song
            Timecode start_song_start;    //the start of the current song

            //The track with songs must be the first one
            AudioTrack track_with_songs = (AudioTrack)vegas.Project.Tracks[0];

            //create a new track for the numbers
            VideoTrack new_track = new VideoTrack(0, "Numerotation");
            vegas.Project.Tracks.Add(new_track);

            foreach (TrackEvent song_event in track_with_songs.Events)
            {
                curr_song_lenght = song_event.Length;
                start_song_start = song_event.Start;

                //create the video event with the corresponding number of the song
                VideoEvent new_event = new VideoEvent(start_song_start, curr_song_lenght);

                PlugInNode plugIn = vegas.Generators.FindChildByUniqueID("{Svfx:com.sonycreativesoftware:titlesandtext}");
                Media media = new Media(plugIn);
                new_track.Events.Add(new_event);
                MediaStream stream = media.Streams[0];
                Take take = new Take(stream);
                new_event.Takes.Add(take);



                //Set the text
                Effect effect = new_event.ActiveTake.Media.Generator;
                OFXEffect fxo = effect.OFXEffect;
                OFXStringParameter tparm = (OFXStringParameter)fxo.FindParameterByName("Text");
                RichTextBox rtfText = new RichTextBox();



                
                rtfText.Text = num.ToString();  //the text


                rtfText.SelectAll();
                Font font= new Font(Constants.FONT_FAMILY, Constants.TEXT_SIZE, FontStyle.Bold);  //font and size

                rtfText.SelectAll();
                rtfText.SelectionColor = Color.Cyan;//Constants.TEXT_COLOR;
                rtfText.SelectionFont = font;

                tparm.Value = rtfText.Rtf;
                fxo.AllParametersChanged();


                num++;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }


    }


}