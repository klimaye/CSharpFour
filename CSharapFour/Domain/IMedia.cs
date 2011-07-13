using System;

namespace CSharapFour.Domain
{
    public interface IMedia
    {
        void Play();
    }

    public class Music : IMedia
    {
        private readonly string _fileName;

        public Music(string fileName)
        {
            _fileName = fileName;
        }

        public void Play()
        {
            //do nothing
        }
    }

    public class Video : IMedia
    {
        private readonly string _fileName;

        public Video(string fileName)
        {
            _fileName = fileName;
        }

        public void Play()
        {
            //do nothing right now.
        }
    }
}