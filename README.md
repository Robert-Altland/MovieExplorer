⚠️Important Note: work in progress

This solution demonstrates a simple movie explorer app, which lets you view a collection of movies, select a movie to view details, play trailers, view similar movies and optionally save movies to your favorites list. 

###Notes about this solution:
* PCL for common code
* Xamarin shared project for linked files that need to be shared across platforms but which don't work in a PCL
* Platform specific class libraries for unit-testable components
* Very light unit testing
* Task based asynchronous data service which handles interaction with the TMDb API (https://www.themoviedb.org)
* A simple cache service for limited offline capability (raw json for simplicity)
* A simple dependency container
* A fairly simple Netflix style user interface (favorites are presented as a category on the home screen)
* iOS and Android apps (phone, not tested on tablet) 
* Project artifacts like requirements, sketches, and comps (no prototype was needed because of the simple UI)

###Critical Issues:
* The image loading performance on Android is close to unusable on device, emulator noticeably slow
* The app hangs on the custom splash screen on iPhone device, simulator OK 

iOS simulator and Genymotion HTC One emulator are currently the primary development devices, starting to include iPhone 6 and HTC One, more coverage as needed. 
