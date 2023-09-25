import { SPlatformBaseTemplatePage } from './app.po';

describe('ELog App', function() {
  let page: SPlatformBaseTemplatePage;

  beforeEach(() => {
    page = new SPlatformBaseTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
