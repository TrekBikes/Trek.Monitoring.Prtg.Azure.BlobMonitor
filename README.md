[![Build Status](https://travis-ci.org/TrekBikes/Trek.Monitoring.Prtg.Azure.BlobMonitor.svg?branch=master)](https://travis-ci.org/TrekBikes/Trek.Monitoring.Prtg.Azure.BlobMonitor)

# PRTG Azure Blob Monitor

A custom EXE PRTG monitor that checks a container in Azure Blob Storage for the following
* Number of Blobs
* Number of Blobs matching a specific pattern
* Total size of Blobs
* Total size of Blobs matching a specific pattern

## Required Parameters

`-N [StorageAccountName] -K "[StorageAccountKey]"`

## All Parameters

Option | Description
------------ | -----------
StorageAccountName* (`-N`) | The name of the storage account
StorageAccountKey* (`-K`) | The storage account's key
ContainerName (`-C`) | _Optional_: The name of the container to look in.  If not provided will look into the most recently modified container
BlobNamePattern (`-B`) | _Optional_: A RegEx to describe the pattern of name of the blobs to look for [Default=`.*`]
MatchInSubdirectories (`-S`) | _Optional_: Should the results include blobs found in virtual directories under the container [Default=`True`]
ModifiedAfter (`-MA`) | _Optional_: A `TimeSpan` string descibing how long ago to look back
ModifiedBefore (`-MB`) | _Optional_: A `TimeSpan` string descibing how long ago to stop looking
