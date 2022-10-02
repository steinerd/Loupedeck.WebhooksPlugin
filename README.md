# Webhooks Loupedeck Plugin
[![License](http://img.shields.io/:license-MIT-blue.svg?style=flat)](LICENSE)
![forks](https://img.shields.io/github/forks/Steinerd/Loupedeck.WebhooksPlugin?style=flat)
![stars](https://img.shields.io/github/stars/Steinerd/Loupedeck.WebhooksPlugin?style=flat)
![issues](https://img.shields.io/github/issues/Steinerd/Loupedeck.WebhooksPlugin?style=flat)
[![downloads](https://img.shields.io/github/downloads/Steinerd/Loupedeck.WebhooksPlugin/latest/total?style=flat)](https://github.com/Steinerd/Loupedeck.WebhooksPlugin/compare)

There is seemingly no built-in way to trigger a "call-and-forget" webhook within the native Loupedeck application. 

The Webhooks Loupedeck Plugin corrects the obvious omission of fundamental functionality in a macro controller. 

## Credits

This plugin makes heavy use of [HarSharp](https://github.com/giacomelli/HarSharp), and its dependancies. 

--------

# Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Support](#support)
- [Contributing](#contributing)
- [License](#license)

# Installation

<details><summary><b>Loupedeck Installation</b></summary>
  
  
  1. Go to [latest release](https://github.com/Steinerd/Loupedeck.WebhooksPlugin/releases/latest), and download the `lplug4` file to you computer
  1. Open (normally double-click) to install, the Loupedeck software should take care of the rest
  1. Restart Loupedeck (if not handled by the installer)
  1. In the Loupedeck interface, enable **Webhooks** by clicking <ins>Manage plugins</ins>
  1. Check the Webhooks box on to enable
  1. Drag the desired control onto your layout

Once click it will bring you to a dynamic playback device selection page. 
</details>

<details><summary><b>IDE Installation</b></summary>
  Made with Visual Studio 2022, C# will likely only compile in VS2019 or greater. 

  Assuming Loupedeck is already installed on your machine, make sure you've stopped it before you debug the project. 

  Debugging _should_ build the solution, which will then output the DLL, config, and pdb into your `%LocalAppData%\Loupedeck\Plugins` directory.

  If all goes well, Loupedeck will then open and you can then debug. 

</details>

# Usage

Follow the __Loupedeck Installation__ instructions above. 

You will need to familiarize yourself with HAR/Http Archive files. They're effectively just JSON files with specific schema for HTTP requests and their responses.

There are numerous ways to create them, or export them in many different applications. Including Postman, Telerik Fiddler2, and even most browsers DevTools will allow you to export/copy web requests as HARs. 

An example HAR file for a fake IFTTT call can be found here: [example.har](example.har)

__All HAR Files must be saved to `%userprofile%/.loupedeck/webhooks` (Windows) -OR- `~/.loupedeck/webhooks` (Mac)__

You can have multiple `*.har` files with multiple requests, or one `.har` with all the requests. The plugin will treat them the same. 

Fields that deal in "size" or "times" can be set to 0. They're not used in the creation/execution of the requests. 

Once completed you will be able to add requests to the *Webhook actions* "Requsts" folder in the Loupedeck UI.

1. Simply click the [+] button on the same row
	1. Skip the name it just gets clobbered the moment you select a hook
1. Select HTTP Method (you will only available ones from your HAR files)
1. Select Hook from the final dropdown and click Save

The button generation leads a lot to be desired, I apologize. I personally just create a macro and drag it in as an action step. 

# Support

[Submit an issue](https://github.com/Steinerd/Loupedeck.WebhooksPlugin/issues/new)

Fill out the template to the best of your abilities and send it through. 

# Contribute

Easily done. Just [open a pull request](https://github.com/Steinerd/Loupedeck.WebhooksPlugin/compare). 

Don't worry about specifics, I'll handle the minutia. 

# License
The MIT-License for this plugin can be reviewed at [LICENSE](LICENSE) attached to this repo.
