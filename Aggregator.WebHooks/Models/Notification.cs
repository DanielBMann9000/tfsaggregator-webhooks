﻿using Aggregator.Core.Interfaces;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using System;
using System.Linq;

namespace Aggregator.Models
{
    /// <summary>
    /// Notification implementation for the Console Application
    /// </summary>
    public class Notification : INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        /// <param name="workItemId">
        /// WorkItemId of the work item to load and apply the policy on.
        /// </param>
        /// <param name="teamProjectCollectionUrl">
        /// Url of Team Project Collection that holds this work item.
        /// </param>
        /// <param name="projectName">
        /// The name of the project that holds this work item.
        /// </param>
        public Notification(int workItemId, string teamProjectCollectionUrl, string projectName)
        {
            this.WorkItemId = workItemId;
            //HACK
            this.ProjectUri = teamProjectCollectionUrl + "/" +projectName;
        }

        /// <summary>
        /// WorkItemId of the work item to load and apply the policy on.
        /// </summary>
        public int WorkItemId { get; }

        /// <summary>
        /// The name of the project that holds the work item.
        /// </summary>
        public string ProjectUri { get; }

        public string ChangerTeamFoundationId
        {
            get
            {
                // we do not implement impersonation... at the moment
                return string.Empty;
            }
        }
    }
}
