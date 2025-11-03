let
  nixpkgs = fetchTarball {
    url = "https://github.com/NixOS/nixpkgs/tarball/nixos-25.05";
  };

  pkgs = import nixpkgs { 
    config = {}; 
    overlays = []; 
  };
in

pkgs.mkShellNoCC {
  packages = with pkgs; [
    # Editors & tools
    neovim
    tmux

    # Frontend development
    nodejs_22
    pnpm

    # Backend development
    dotnetCorePackages.sdk_10_0-bin

    # DevOps & Infrastructure
    ansible
  ];
}
