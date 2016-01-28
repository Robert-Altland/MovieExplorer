# Movie Explorer
A sample solution demonstrating best practices for architecting cross platform mobile applications using Xamarin.

#Overview
Building cross platform mobile applications using Xamarin is a good way for a .NET developer to become familiar with the architecture and frameworks of the iOS and Android platforms without getting impeded by language syntax and development tools. 

This solution demonstrates a simple movie explorer app, which lets you view a collection of movies, select one to view details, play trailers, view similar movies and optionally save movies to your favorites list. 

#Highlights of this solution:
* PCL for common code
* Xamarin shared project for linked files that need to be shared across platforms but which don't work in a PCL
* Platform specific class libraries for unit-testable components
* Advanced customization of UI elements
* A cache service for limited offline capability

#Design:
* I paid some attention to writing a sample app that looks and works well across a variety of screen resolutions and dimensions, but focused primarily on phone 
* I settled on a Netflix style UI, showing categories of movie lists which include a user defined list (favorites) as opposed to creating separate views to interact with your favorites

#Scope of development:
* I focused on simulators. At the last minute I tried to run on devices and found that the application doesn't function as expected. I'm working with Xamarin support to understand the cause of the error, but I suspect PCL targeting
* I tried to document this development as a real world evolution from envisioning a solution through to delivering it. This includes synthesizing the original requirements document into user stories, sketches and comps of possible UI/UI concepts, scrum reports and committing work to this repository
* I started writing unit tests to show TDD in action, but because this isn't the focus of the demo, I opted to not continue in order to focus on other aspects of this app
* I implemented a very simple cache service to cache the raw json responses from API calls. I leveraged this to persist favorite selections across app restarts. I could have used SQLite to be more efficient with offline storage, but I put a lot of time into the user experience and this became secondary
* I purposely didn’t pull in any code I’ve written from other projects, so except for one third party library’s source, all code has been written especially for this

#Important notes:
* The impact of PCL targeting can’t be emphasized enough. I suspect this is the cause of my inability to debug this project on device at the present time, and the cause of the hang when running on a device. I expect this to be resolved shortly and will provide an overview of the changes
