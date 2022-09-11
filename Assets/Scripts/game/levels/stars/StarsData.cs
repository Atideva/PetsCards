using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace game.levels.stars
{
    [System.Serializable]
    public class StarsData
    {
        List<string> _levelNames = new List<string>();

        #region modifiers

        public List<string> SetLevelNames => _levelNames;

        public ReadOnlyCollection<string> LevelNames => _levelNames.AsReadOnly();

        #endregion

        List<int> _starsEarned = new List<int>();

        #region modifiers

        public List<int> SetStarsEarned => _starsEarned;

        public ReadOnlyCollection<int> StarsEarned => _starsEarned.AsReadOnly();

        #endregion

        List<int> _pointsRequiredForOneStar = new List<int>();
        List<int> _pointsRequiredForTwoStars = new List<int>();
        List<int> _pointsRequiredForThreeStars = new List<int>();

        #region modifiers

        public List<int> SetPointsRequiredForOneStar => _pointsRequiredForOneStar;

        public ReadOnlyCollection<int> PointsRequiredForOneStar => _pointsRequiredForOneStar.AsReadOnly();

        #endregion

        #region modifiers

        public List<int> SetPointsRequiredForTwoStars => _pointsRequiredForTwoStars;

        public ReadOnlyCollection<int> PointsRequiredForTwoStars => _pointsRequiredForTwoStars.AsReadOnly();

        #endregion

        #region modifiers

        public List<int> SetPointsRequiredForThreeStars => _pointsRequiredForThreeStars;

        public ReadOnlyCollection<int> PointsRequiredForThreeStars => _pointsRequiredForThreeStars.AsReadOnly();

        #endregion

        List<int> _timeRemainingForOneStar = new List<int>();
        List<int> _timeRemainingForTwoStars = new List<int>();
        List<int> _timeRemainingForThreeStars = new List<int>();

        #region modifiers

        public List<int> SetTimeRemainingForOneStarr => _timeRemainingForOneStar;

        public ReadOnlyCollection<int> TimeRemainingForOneStar => _timeRemainingForOneStar.AsReadOnly();

        #endregion

        #region modifiers

        public List<int> SetTimeRemainingForTwoStars => _timeRemainingForTwoStars;

        public ReadOnlyCollection<int> TimeRemainingForTwoStars => _timeRemainingForTwoStars.AsReadOnly();

        #endregion

        #region modifiers

        public List<int> SetTimeRemainingForThreeStars => _timeRemainingForThreeStars;

        public ReadOnlyCollection<int> TimeRemainingForThreeStars => _timeRemainingForThreeStars.AsReadOnly();

        #endregion
    }
}