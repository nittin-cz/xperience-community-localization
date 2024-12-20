const webpackMerge = require('webpack-merge');
const baseWebpackConfig = require('@kentico/xperience-webpack-config');

module.exports = (opts) => {
  const baseConfig = (webpackConfigEnv, argv) => {
    return baseWebpackConfig({
      orgName: 'nittin',
      projectName: 'xperience-community-localization',
      webpackConfigEnv: webpackConfigEnv,
      argv: argv,
    });
  };

  const projectConfig = {
    module: {
      rules: [
        {
          test: /\.(js|ts)x?$/,
          exclude: [/node_modules/],
          loader: 'babel-loader',
        },
      ],
    },
    devServer: {
      port: 3011,
    },
  };

  return webpackMerge.merge(projectConfig, baseConfig(opts));
};