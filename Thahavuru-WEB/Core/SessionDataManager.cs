///<header>
///  <title>SessionDataManager</title>
///  <description>
///  SessionDataManager
///  </description>
///  <copyRight>Copyright (c) 2012</copyRight>
///  <company>Embla software Innovation (Pvt)Ltd</company>
///  <createdOn>2012-09-26</createdOn>
///  <author>Eranda</author>
///  <Comments></Comments>
///  <modifiedBy></modifiedBy>
///  <modifiedDate></modifiedDate>
///  <description></description>
///</header>

#region Using directives

using System;
using System.Web;

#endregion

namespace DADAS.Core
{
    public class SessionDataManager
    {
        [Serializable]
        public enum Key
        {
            isDay,
            currentAircraft,
            maxX
        }

        #region Properties

        /// <summary>
        /// Gets a value indicating whether session state has been initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if session state has been initialized; 
        /// otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get
            {
                return (HttpContext.Current.Session != null);
            }
        }

        #endregion

        #region Class Members

        /// <summary>
        /// Ensure key is valid in session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExists(SessionDataManager.Key key)
        {
            if (HttpContext.Current.Session[key.ToString()] != null)
            {
                return true;
            }
            //.Current.Session
            return false;
        }

        /// <summary>
        /// Ensures the session is valid.
        /// </summary>
        private void EnsureSessionIsValid()
        {
            // check whether the session has not been initialized
            if (!(this.IsInitialized))
            {
                // throw exception
                throw new Exception("Session Is Invalid");
            }
        }

        /// <summary>
        /// Set the given value identified by the specified key, in session data.
        /// </summary>
        /// <param name="key">
        /// The key used to identify the session data item.
        /// </param>
        /// <param name="value">
        /// The value to be stored in session data.
        /// </param>
        public void Set(SessionDataManager.Key key, object value)
        {
            // ensure the session is valid
            this.EnsureSessionIsValid();

            HttpContext.Current.Session[key.ToString()] = value;
        }

        /// <summary>
        /// Gets the value identified by the specified key.
        /// </summary>
        /// <param name="key">
        /// The key identifying the value to be retrieved.
        /// </param>
        /// <returns>
        /// The value identified by <paramref name="key"/>.
        /// </returns>
        public object Get(SessionDataManager.Key key)
        {
            // ensure the session is valid
            this.EnsureSessionIsValid();

            return HttpContext.Current.Session[key.ToString()];
        }

        /// <summary>
        /// Remove the session data using the given session key
        /// </summary>
        /// <param name="key">
        /// The key used to identify the session data item.
        /// </param>
        public void Clear(SessionDataManager.Key key)
        {
            // ensure the session is valid
            this.EnsureSessionIsValid();

            if (HttpContext.Current.Session[key.ToString()] != null)
            {
                // Remove the session data using the given session key
                HttpContext.Current.Session.Remove(key.ToString());
            }
        }

        #endregion
    }
}