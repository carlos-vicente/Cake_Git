using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Git.Extensions;
using LibGit2Sharp;
// ReSharper disable UnusedMember.Global

namespace Cake.Git
{
    /// <summary>
    /// Class GitAliases.
    /// </summary>
    public static partial class GitAliases
    {
        /// <summary>
        /// Gets the current branch.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryDirectoryPath">Path to repository.</param>
        /// <returns>GitBranch.</returns>
        /// <exception cref="ArgumentNullException">context
        /// or
        /// repositoryDirectoryPath</exception>
        /// <example>
        ///   <code>
        /// var repositoryDirectoryPath = DirectoryPath.FromString(".");
        /// var currentBranch = ((ICakeContext)cakeContext).GitBranchCurrent(repositoryDirectoryPath);
        ///   </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Branch")]
        public static GitBranch GitBranchCurrent(
            this ICakeContext context,
            DirectoryPath repositoryDirectoryPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (repositoryDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(repositoryDirectoryPath));
            }

            return context.UseRepository(
                repositoryDirectoryPath,
                repository => new GitBranch(repository)
                );
        }

        /// <summary>
        /// Creates a new branch with default settings
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryDirectoryPath">Path to repository.</param>
        /// <param name="branchName">The branch's name</param>
        /// <returns>GitBranch which just got created</returns>
        /// <exception cref="ArgumentNullException">context or repositoryDirectoryPath</exception>
        /// <example>
        ///   <code>
        /// var repositoryDirectoryPath = DirectoryPath.FromString(".");
        /// var currentBranch = ((ICakeContext)cakeContext).GitCreateBranch(repositoryDirectoryPath);
        ///   </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Branch")]
        public static GitBranch GitCreateBranch(
            this ICakeContext context,
            DirectoryPath repositoryDirectoryPath,
            string branchName)
        {
            var defaultSettings = new GitBranchSettings();
            return GitCreateBranch(context, repositoryDirectoryPath, branchName, defaultSettings);
        }

        /// <summary>
        /// Creates a new branch with the given settings
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryDirectoryPath">Path to repository.</param>
        /// <param name="branchName">The branch's name</param>
        /// <param name="settings"></param>
        /// <returns>GitBranch which just got created</returns>
        /// <exception cref="ArgumentNullException">context or repositoryDirectoryPath</exception>
        /// <example>
        ///   <code>
        /// var repositoryDirectoryPath = DirectoryPath.FromString(".");
        /// var currentBranch = ((ICakeContext)cakeContext).GitCreateBranch(repositoryDirectoryPath);
        ///   </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Branch")]
        public static GitBranch GitCreateBranch(
            this ICakeContext context,
            DirectoryPath repositoryDirectoryPath,
            string branchName,
            GitBranchSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (repositoryDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(repositoryDirectoryPath));
            }

            return context.UseRepository(
                repositoryDirectoryPath,
                repository => {
                    var head = repository.Head.Tip;

                    context.Log.Write(
                        Core.Diagnostics.Verbosity.Normal,
                        Core.Diagnostics.LogLevel.Information,
                        "Going to create a new branch with name {0}",
                        branchName);

                    //foreach(var branch in repository.Branches)
                    //{
                    //    context.Log.Write(
                    //        Core.Diagnostics.Verbosity.Normal,
                    //        Core.Diagnostics.LogLevel.Information,
                    //        "Existing branch: {0}",
                    //        branch.CanonicalName);
                    //}

                    var newBranch = repository
                        .Branches
                        .Add(branchName, head, settings.AllowOverwrite);

                    context.Log.Write(
                        Core.Diagnostics.Verbosity.Normal,
                        Core.Diagnostics.LogLevel.Information,
                        "Branch with name {0} created",
                        branchName);

                    if (settings.TrackOnRemote)
                    {
                        repository
                            .Branches
                            .Update(newBranch, updater =>
                            {
                                updater.TrackedBranch = $"refs/remotes/{settings.Remote.Name}/{settings.Remote.BranchName ?? branchName}";
                            });
                    }

                    if (settings.CheckoutNewBranch)
                    {
                        Commands.Checkout(repository, newBranch);
                    }

                    return new GitBranch(repository);
                });
        }
    }
}
