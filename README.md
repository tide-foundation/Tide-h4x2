# The Tide H4X challenge
The [H4X challenge](http://h4x2.tide.org) is a showcase of the Tide Protocol's novel user authentication and digital protection technology, inviting the online community to learn, contribute and engage with Tide in the development of the protocol. It also encourages participants to identify and report security flaws, improvements or fixes via a bounty offer.

# H4X.2
This [H4X2](http://h4x2.tide.org) challenge is the second series of the community-engagement program by the [Tide Foundation](https://tide.org) with a specific focus on Tide's next-generation technology: A new technology to grant access using keys nobody holds. In this series, the challenge will change and evolve according to the community engagement, and will gradually introduce additional facets of the technology.

## Challenge Mechanics
The concept of the first challenge is simple.  A secret code is hidden and is only unlocked when the correct password is entered.  The first one to post the secret code on the Tide's #general channel on its discord server - wins!  The password authentication process is obfuscated and decentralized using Tide's [PRISM](https://github.com/tide-foundation/Tide-h4x2/blob/main/diagrams/svg/H4x2_prism.svg) cryptography - the world's most secure password authentication[^pwd].  In this challenge, only two nodes perform the authentication.  One node will be completely exposed and offers full transparancy to its internal data and processes while the other node remains private.  The entire source code for the challenge, together with full documentation, is offered herewith for those wishing to take a deeper look.  The user flow can be found below and the full technical diagram can be found [here](https://github.com/tide-foundation/Tide-h4x2/blob/main/diagrams/svg/H4x2_Challenge.svg).

## User Flow Diagram
![alt text](https://github.com/tide-foundation/Tide-h4x2/blob/main/diagrams/svg/H4x2_userflow.svg "Flow Diagram")

## Components
1. **H4x2-Node** - Small version of the Tide node specific to this challenge.
1. **H4x2-TinySDK** - Small SDK for frontend website integration.
1. **H4x2-TideJS** - Tide Libraries including encryption
1. **H4x2-front** - Frontend website for this challenge.
1. **diagrams** -  diagrams for this challenge.
    1. **H4x2_Challenge** - A technical diagram of the challenge.  
    2. **H4x2_prism** - The mathematical diagram of Tide's PRISM. 
    3. **H4x2_userflow** - User flow diagram. 

# Installation
This guide aims to assist you to replicate the entire challenge environment locally, with 2 nodes - so you can run it yourself freely.

While all the components of the this environment are cross-platform, this manual describes how to set it up in a Windows environment. Similar steps can be followed to achieve the same on Linux.

### Prerequisite

The following components are required to be set up ahead of the deployment:
1. [.NET Core 2.2 Build apps - SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2 ".net Core 2.2 Download")
1. [Node.js - LTS](https://nodejs.org/en/download/ "node.js Download")
1. Clone Repository `git clone https://github.com/tide-foundation/Tide-h4x2/`

#### Installing Node.js

### Deployment

#### Credential Generation

#### Configuring settings

#### Frontend Setup

# More info
[The Tide Website](https://tide.org)

## Get in touch!

[Tide Discord](https://discord.gg/42UCeW4smw)

[Tide Subreddit channel](https://www.reddit.com/r/TideFoundation)

  <a href="https://tide.org/licenses_tcosl-1-0-en">
    <img src="https://img.shields.io/badge/license-TCOS-green.svg" alt="license">
  </a>
</p>

[^pwd]: Tide has developed the world's most secure online password authentication mechanism because it is still, unfortunately, the most common online authentication mechanism to-date. In general, password authentication is a significantly inferior mechanism compared to its many alternatives. Most of the alternatives (e.g. MFA, passwordless, FIDO2, etc) also suffer from security risks which Tide's authentication helps alleviating. Tide's superior password protection mechanism isn't intended to discourage users from switching to a better alternatives, instead offers a better half-measure until such inevitable switch occurs.
