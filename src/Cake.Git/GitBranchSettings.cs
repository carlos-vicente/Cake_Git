namespace Cake.Git
{
    /// <summary>
    /// Settings used to describe which parameters will be used when creating a new branch
    /// </summary>
    public class GitBranchSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public class GitBranchRemote
        {
            /// <summary>
            /// 
            /// </summary>
            public GitBranchRemote() : this(null) { }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="branchName"></param>
            public GitBranchRemote(string branchName) : this("origin", branchName) { }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="branchName"></param>
            public GitBranchRemote(string name, string branchName)
            {
                Name = name;
                BranchName = branchName;
            }

            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string BranchName { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public GitBranchSettings() : this(new GitBranchRemote()) { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remote"></param>
        public GitBranchSettings(GitBranchRemote remote)
        {
            Remote = remote;
        }

        /// <summary>
        /// Allow silently overwritting a potencially existing branch with the defined name
        /// </summary>
        public bool AllowOverwrite { get; set; }

        /// <summary>
        /// Indicates if the created branch should track a remote one
        /// </summary>
        public bool TrackOnRemote { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GitBranchRemote Remote { get; set; }
    }
}
